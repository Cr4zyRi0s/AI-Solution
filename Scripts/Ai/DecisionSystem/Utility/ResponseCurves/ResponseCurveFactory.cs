using System;
namespace AI.DecisionSystem.Utility
{
    public class ResponseCurveFactory
    {
        public static IResponseCurve GetResponseCurve(ResponseCurveType type, float slope, float exponent, float xOffset, float yOffset) {
            switch (type) {
                case ResponseCurveType.LINEAR:
                    return new LinearResponseCurve(slope,exponent,xOffset,yOffset);
                case ResponseCurveType.LOGISTIC:
                    return new LinearResponseCurve(slope, exponent, xOffset, yOffset);
                case ResponseCurveType.LOGIT:
                    return new LinearResponseCurve(slope, exponent, xOffset, yOffset);
                default:
                    throw new Exception("Got invalid ResponseCurveType");
            }
        }
    }
}