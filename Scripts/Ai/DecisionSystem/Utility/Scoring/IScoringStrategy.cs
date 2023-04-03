using System.Collections.Generic;

namespace AI.DecisionSystem.Utility.Scoring
{
    public interface IScoringStrategy
    {
        double Score(IEnumerable<double> axisValues, double weight = 1d);
    }
}