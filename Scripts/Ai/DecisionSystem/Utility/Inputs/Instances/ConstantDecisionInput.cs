using UnityEditor;
using UnityEngine;

namespace AI.DecisionSystem.Utility.Inputs
{
    public class ConstantDecisionInput : DecisionInputBase
    {
        public override double Value => 1d;
    }
}