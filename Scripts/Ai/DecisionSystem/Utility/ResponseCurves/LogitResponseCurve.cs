using System;

namespace AI.DecisionSystem.Utility
{
    [Serializable]
    public class LogitResponseCurve : BaseResponseCurve
    {
        public LogitResponseCurve(float Slope, float Exponent, float XOffset, float YOffset) : base(Slope, Exponent, XOffset, YOffset)
        {
        }

        public override double Evaluate(double input)
        {
            return Utils.Sanitize(Slope * Math.Log((input - XOffset) / (1.0 - (input - XOffset))) / 5.0 + 0.5 + YOffset);
        }
    }
}