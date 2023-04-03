using System;
using System.Collections;
using UnityEngine;
using AI.DecisionSystem.Utility;
using AI.DecisionSystem.Utility.Inputs;
using AI.DecisionSystem.Utility.Scoring;

namespace AI.Action
{
    public class UtilityBasedActionProviderFactory 
    {
        public static UtilityBasedActionProvider GetUtilityBasedActionProvider(AIAgent agent, UtilityActionProviderType type) {
            switch (type) 
            {
                case UtilityActionProviderType.SIMPLE_WANDER:
                    return BuildSimpleWanderActionProvider(agent);
                default:
                    throw new ArgumentException("Invalid UtilityBasedActionProvider type");
            }
        }

        private static UtilityBasedActionProvider BuildSimpleWanderActionProvider(AIAgent agent)
        {
            var utilityBuilder = new UtilityDecisionSystemBuilder();
            var actionContext = new ActionContext(agent);
            
            var wanderChoice = new UtilityDecisionChoiceBuilder()
                .AddAxis(
                    DecisionInputType.CONSTANT,
                    DecisionInputParams.std,
                    ResponseCurveType.LINEAR,
                    ResponseCurveParams.passthrough
                    )
                .SetWeight(1d)
                .SetScoringStrategy(ScoringStrategyType.ADD)
                .Build();
            var wanderAction = ActionFactory.GetAction(actionContext, ActionType.MOVE_TO);
            utilityBuilder.AddChoice(wanderChoice, wanderAction);

            return new UtilityBasedActionProvider(agent,utilityBuilder.Build());
        }
    }

    public enum UtilityActionProviderType 
    {
        SIMPLE_WANDER,
    }
}