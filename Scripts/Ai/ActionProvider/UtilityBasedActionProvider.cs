using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using AI.DecisionSystem.Utility;

namespace AI.Action
{
    public class UtilityBasedActionProvider : IActionProvider
    {
        private UtilityDecisionSystem _utilityDecisionSystem;
        private AIAgent _agent;
        private IAction _runningAction = null;

        public IAction Action
        {
            get
            {
                if (_runningAction == null || _runningAction.State != ActionState.RUNNING)
                {
                    _runningAction = _utilityDecisionSystem.Selection as IAction;
                }
                return _runningAction;
            }
        }

        public UtilityBasedActionProvider(AIAgent agent, UtilityDecisionSystem utilityDecisionSystem) {
            _agent = agent;
            _utilityDecisionSystem = utilityDecisionSystem;
        }
    }
}