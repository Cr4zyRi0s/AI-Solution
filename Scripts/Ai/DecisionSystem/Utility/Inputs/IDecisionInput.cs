
namespace AI.DecisionSystem.Utility.Inputs
{
    public interface IDecisionInput
    {
        public abstract double NormalizedValue { get; }
        public abstract double Value { get; }
        public abstract double Min { get; set; }
        public abstract double Max { get; set; }
    }
}