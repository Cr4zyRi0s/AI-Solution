using System.Collections.Generic;

namespace AI.DecisionSystem.Utility.Selection
{
    public interface ISelectionStrategy
    {
        IDecisionChoice Select(IEnumerable<IDecisionChoice> choices);
    }
}