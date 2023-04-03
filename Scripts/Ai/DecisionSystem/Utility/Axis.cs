using System;
using AI.DecisionSystem.Utility.Inputs;

namespace AI.DecisionSystem.Utility
{
    [Serializable]
    public class Axis
    {
        public Axis(
            IDecisionInput input,
            IResponseCurve curve,
            string name = null)
        {            
            Input = input;
            Curve = curve;
            _name = name;
        }

        public double NormalizedValue => Curve.Evaluate(Input.NormalizedValue);

        private string _name;
        public string Name => _name ?? GetType().Name;            
        public IDecisionInput Input { get; private set; }
        public IResponseCurve Curve { get; private set; }
    }
}