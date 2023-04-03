public struct ResponseCurveParams
{
    public float Slope;
    public float Exponent;
    public float XOffset;
    public float YOffset;

    public static ResponseCurveParams passthrough => new ResponseCurveParams(1,1);
    public ResponseCurveParams(float slope, float exponent) : this()
    {
        Slope = slope;
        Exponent = exponent;
        XOffset = 0f;
        YOffset = 0f;
    }

    public ResponseCurveParams(float slope, float exponent, float xOffset, float yOffset)
    {
        Slope = slope;
        Exponent = exponent;
        XOffset = xOffset;
        YOffset = yOffset;
    }
}