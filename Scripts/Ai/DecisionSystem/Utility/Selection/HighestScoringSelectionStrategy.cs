using System;
using System.Linq;
using System.Collections.Generic;

namespace AI.DecisionSystem.Utility.Selection 
{
    [Serializable]
    public class HighestScoringSelectionStrategy : ISelectionStrategy
    {
        public IDecisionChoice Select(IEnumerable<IDecisionChoice> choices)
        {
            return choices.Select(choice => choice as UtilityDecisionChoice).Aggregate((a,b) => (a.Score > b.Score)? a : b);
        }
    }
}