namespace AI.DecisionSystem.Utility
{
    public interface IResponseCurve
    {
        public float Exponent { get; set; }
        public float Slope { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }

        double Evaluate(double input);
    }
}