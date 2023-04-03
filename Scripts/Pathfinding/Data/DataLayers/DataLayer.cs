using UnityEngine;

namespace CustomAiNavigation.Zone.Data
{
    public abstract class DataLayer<T>
    {
        public bool IsStatic = true;
        protected Vector3 projectionVector = Vector3.up;
        protected Polygon[] cachedIsometricRegions;        


        protected DataLayer(bool isStatic)
        {
            IsStatic = isStatic;
        }

        public abstract void SetData(NavigationSample sample, T data);
        public abstract T GetData(NavigationSample sample);
        public abstract T GetDataInterp(NavigationSample sample0, NavigationSample sample1, float t);
        public abstract Polygon[] GetIsometricRegions(NavigationSample forSample);        

    }
}