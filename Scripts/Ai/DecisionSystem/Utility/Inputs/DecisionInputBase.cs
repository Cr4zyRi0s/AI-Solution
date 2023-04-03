using System;

namespace AI.DecisionSystem.Utility.Inputs
{
    [Serializable]
    public abstract class DecisionInputBase : IDecisionInput
    {
        public DecisionInputBase(string name = null)
        {
            _name = name;
        }

        private string _name;
        public string Name => _name ?? GetType().Name;

        public abstract double Value { get; }

        public virtual double NormalizedValue => Math.Clamp((Value - Min) / (Max - Min),0f,1f);

        public virtual double Min { get; set; }
        public virtual double Max { get; set; }
    }
}