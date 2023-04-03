using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAiNavigation.Zone.Data{

    public class RelativeBooleanDataLayer : DataLayer<CompressedBooleanList>{
        
        public Dictionary<NavigationSample,CompressedBooleanList> LayerData;


        public RelativeBooleanDataLayer(List<NavigationSample> samples, bool isStatic = false)  : base (isStatic)
        {
            this.LayerData = new Dictionary<NavigationSample, CompressedBooleanList>();
            foreach (var sample in samples)
            {
                this.LayerData[sample] = new CompressedBooleanList(samples.Count);
            }
        }

        public CompressedBooleanList this[NavigationSample sample]{
            get { return this.GetData(sample);}
        }

        public bool this[NavigationSample sample0, NavigationSample sample1]{
            get { return GetData(sample0)[sample1.id];}
            set { LayerData[sample0][sample1.id] = value;}
        }

        public override CompressedBooleanList GetData(NavigationSample sample)
        {
            return LayerData[sample];
        }

        public override CompressedBooleanList GetDataInterp(NavigationSample sample0, NavigationSample sample1, float t)
        {
            if (t < 0.5f)
                return LayerData[sample0];
            else
                return LayerData[sample1];
        }

        public override Polygon[] GetIsometricRegions(NavigationSample forSample)
        {
            if (this.IsStatic)
            {
                if (cachedIsometricRegions != null)
                    return cachedIsometricRegions;
            }
            var regions = new List<Polygon>();

            var sampleBuckets = new Dictionary<int, List<NavigationSample>>();
            foreach (var navSample in this.LayerData.Keys)
            {
                var val = 0;
                if (this[forSample, navSample]) 
                    val = 1;
                var hash = 13 * navSample.gridPosition.x + 17 * navSample.gridPosition.y + 19 * val;

                if (!sampleBuckets.ContainsKey(hash))
                {
                    sampleBuckets[hash] = new List<NavigationSample>();
                }
                sampleBuckets[hash].Add(navSample);                       
            }

            var strips = new Dictionary<int, List<NavigationSample[]>>();
            foreach (var bucket in sampleBuckets.Values)
            {
                bucket.Sort((s0,s1) => (s0.position.z > s1.position.z)? 1 : -1);
                var x = bucket[0].gridPosition.x;
                if (!strips.ContainsKey(x))
                {
                    strips[x] = new List<NavigationSample[]>();
                }
                var lastZ = bucket[0].gridPosition.z;
                var index = 0;
                for (int i = 0; i < bucket.Count; i++)
                {                
                    if (Mathf.Abs(bucket[i].gridPosition.z - lastZ) > 1 || i == bucket.Count - 1)
                    {
                        strips[x].Add(new NavigationSample[] { bucket[index], bucket[i] });
                        index = i;
                    }
                    lastZ = bucket[i].gridPosition.z;
                }
            }

            var sampleSets = new List<HashSet<NavigationSample>>();
            foreach(var x in strips.Keys)
            {                
                if (strips.ContainsKey(x + 1))
                {
                    var nextStrips = strips[x + 1];
                    foreach(var strip in strips[x]) { 
                        foreach(var nStrip in nextStrips)
                        {
                            if (strip[0].gridPosition.z >= nStrip[0].gridPosition.z &&
                                strip[0].gridPosition.z <= nStrip[1].gridPosition.z)
                            {

                            }
                            else if (strip[1].gridPosition.z <= nStrip[1].gridPosition.z)
                            {

                            }
                        }
                    }
                }
            }


            var regionArray = regions.ToArray();
            if(this.IsStatic)
            {
                cachedIsometricRegions = regionArray;
            }
            return regionArray;
        }

        public override void SetData(NavigationSample sample, CompressedBooleanList data)
        {
            LayerData[sample] = data;
        }
    }
}