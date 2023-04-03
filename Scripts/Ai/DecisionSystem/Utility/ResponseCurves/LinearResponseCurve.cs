using System;

namespace AI.DecisionSystem.Utility
{
    [Serializable]
    public class LinearResponseCurve : BaseResponseCurve
    {
        public LinearResponseCurve(float Slope, float Exponent, float XOffset, float YOffset) : base(Slope, Exponent, XOffset, YOffset)
        {}

        public override double Evaluate(double input)
        {
            return Utils.Sanitize((Slope * Math.Pow(input - XOffset, Exponent)) + YOffset);
        }
    }
}