public struct DecisionInputParams 
{
    public double Min;
    public double Max;

    public static DecisionInputParams std => new DecisionInputParams(0d,1d);

    public DecisionInputParams(double min, double max)
    {
        Min = min;
        Max = max;
    }
}