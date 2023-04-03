using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAiNavigation.Zone.Data
{
    public class FloatDataLayer : DataLayer<float>
    {
        public Dictionary<NavigationSample, float> LayerData;

        private float? _maxValue;
        public float MaxValue
        {
            get
            {
                if (_maxValue == null || !IsStatic)
                    return LayerData.Values.Max();
                else
                {
                    if (IsStatic)
                        return _maxValue.Value;
                    else
                        return LayerData.Values.Max();
                }
            }
        }

        private float? _minValue;        
        public float MinValue
        {
            get { 
                if(_minValue == null || !IsStatic)
                    return LayerData.Values.Min();
                else
                {
                    if (IsStatic)
                        return _minValue.Value;
                    else
                        return LayerData.Values.Min();
                }              
            }            
        }

        public FloatDataLayer(bool isStatic) : base(isStatic)
        {
            LayerData = new Dictionary<NavigationSample, float>();
        }

        public float this[NavigationSample sample]
        {
            get { return this.GetData(sample); }
            set { this.SetData(sample,value); }
        }

        public override float GetData(NavigationSample sample)
        {
            return LayerData[sample];
        }

        public override float GetDataInterp(NavigationSample sample0, NavigationSample sample1, float t)
        {
            return Mathf.Lerp(LayerData[sample0], LayerData[sample1], t);
        }

        public override void SetData(NavigationSample sample, float data)
        {
            LayerData[sample] = data;
        }
        public override Polygon[] GetIsometricRegions(NavigationSample forSample)
        {
            throw new System.NotImplementedException();
        }
    }
}