using System;

namespace AI.DecisionSystem.Utility
{
    [Serializable]
    public class LogisticResponseCurve : BaseResponseCurve
    {
        public LogisticResponseCurve(float Slope, float Exponent, float XOffset, float YOffset) : base(Slope, Exponent, XOffset, YOffset)
        {}

        public override double Evaluate(double input)
        {
            return Utils.Sanitize((Slope / (1 + Math.Exp(-10.0 * Exponent * (input - 0.5 - XOffset)))) + YOffset);
        }
    }
}