using UnityEditor;
using UnityEngine;

namespace AI.DecisionSystem.Utility {
    public abstract class BaseResponseCurve : IResponseCurve
    {
        public float Exponent { get; set; }
        public float Slope { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }

        public BaseResponseCurve(float Slope, float Exponent, float XOffset, float YOffset) {
            this.Slope = Slope;
            this.Exponent = Exponent;
            this.XOffset = XOffset;
            this.YOffset = YOffset;
        }

        public abstract double Evaluate(double input);        
    }
}