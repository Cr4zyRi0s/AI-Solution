using UnityEngine;
using KdTree;
using KdTree.Math;
using System.Linq;
using System.Collections.Generic;

namespace CustomAiNavigation.Zone.Data
{
    public class NavigationData
    {
        public List<NavigationSample> Samples { get; private set; }

        //-------------------ACTUAL DATA-------------------------
        public RelativeBooleanDataLayer HeadLevelVisibilityLayer { get; private set; }
        public FloatDataLayer CoverLayer { get; private set; }        


        private KdTree<float, NavigationSample> sampleTree;
        private NavigationZone navZone; 

        public NavigationData(NavigationZone navZone ,List<NavigationSample> samples)
        {
            this.navZone = navZone;
            this.Samples = samples;
            this.HeadLevelVisibilityLayer = new RelativeBooleanDataLayer(samples, true);
            this.CoverLayer = new FloatDataLayer(true);
            //this.FeetLevelVisibilityLayer = new NavigationRelativeBooleanLayer(samples);

            this.sampleTree = new KdTree<float, NavigationSample>(3, new FloatMath());
            foreach (var sample in this.Samples)
            {
                sampleTree.Add(new float[] { sample.position.x, sample.position.y, sample.position.z}, sample);
            }
        }

        public NavigationSample[] GetKNearestSamples(Vector3 toPos, int k) {
            var nodes = sampleTree.GetNearestNeighbours(new[] { toPos.x, toPos.y, toPos.z }, k);
            return nodes.Select((node) => node.Value).ToArray();
        }

        public void ComputeVisibilityData() {
            AgentParameters agentParams = new AgentParameters();
            agentParams.bounds = new Bounds(Vector3.zero, new Vector3(.7f,1.8f,.7f));

            foreach (var sample in Samples)
            {
                foreach (var other in Samples)
                {                     
                    if (sample == other)
                    {
                        HeadLevelVisibilityLayer[sample, other] = true;
                        HeadLevelVisibilityLayer[other, sample] = true;
                        continue;
                    }

                    var startPos = sample.position + Vector3.up * agentParams.height * 0.5f;
                    var endPos = other.position + Vector3.up * agentParams.height * 0.5f;
                    var translatedBounds = new Bounds(endPos,agentParams.bounds.size);
                    if (BoxLOSChecker.Check(startPos,translatedBounds)) {
                        HeadLevelVisibilityLayer[sample, other] = true;
                        HeadLevelVisibilityLayer[other, sample] = true;
                    }
                    else
                    {
                        HeadLevelVisibilityLayer[sample, other] = false;
                        HeadLevelVisibilityLayer[other, sample] = false;
                    }  
                }
            }
        }

        public void ComputeCoverData() {

            var directions = new[] {
                Vector3.forward,
                (Vector3.right + Vector3.forward).normalized,
                Vector3.right,
                (Vector3.right - Vector3.forward).normalized,
                -Vector3.forward,
                -(Vector3.right + Vector3.forward).normalized,
                -Vector3.right,
                (-Vector3.right + Vector3.forward).normalized
            };

            float maxDist = Mathf.Max(navZone.WorldBounds.size.x, navZone.WorldBounds.size.z);
            foreach (var sample in this.Samples)
            {
                var distances = new float[8];
                for (int i = 0; i < directions.Length; i++)                
                {
                    RaycastHit rhit;
                    if (Physics.Raycast(sample.position, directions[i], out rhit, maxDist)) {
                        if (this.navZone.Contains(rhit.point)) {
                            distances[i] = Vector3.Distance(rhit.point, sample.position);
                        }
                    }
                    else
                    {
                        distances[i] = maxDist;
                    }
                }
                var score = distances.Select(dist => Mathf.Pow(2,1 - dist / 5)).Sum();
                CoverLayer[sample] = score;
            }

            var maxScore = CoverLayer.LayerData.Values.Max();
            var minScore = CoverLayer.LayerData.Values.Min();


            foreach(var sample in Samples)
            {
                CoverLayer.SetData(sample,(CoverLayer.GetData(sample) - minScore) / (maxScore - minScore + .001f));
            }
        }

        public void Load(string filename) { 
            
        }

        public void Save(string filename)
        {

        }
    }
}