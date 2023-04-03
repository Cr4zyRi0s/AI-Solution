using System;
using System.Collections;
using UnityEngine;

namespace AI.DecisionSystem.Utility.Scoring
{
    public class ScoringStrategyFactory 
    {
        public static IScoringStrategy GetScoringStrategy(ScoringStrategyType type) {
            switch (type) 
            {
                case ScoringStrategyType.ADD:
                    return new AddScoringStrategy();
                case ScoringStrategyType.MULTIPLY:
                    return new MultiplyScoringStrategy();
                default:
                    throw new ArgumentException("Invalid ScoringStrategy type");
            }
        }
    }
}