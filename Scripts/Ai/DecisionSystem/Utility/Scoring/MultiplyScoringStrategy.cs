using System;
using System.Collections.Generic;

namespace AI.DecisionSystem.Utility.Scoring
{

    [Serializable]
    public class MultiplyScoringStrategy : IScoringStrategy
    {
        public double Score(IEnumerable<double> axisValues, double weight)
        {
            var result = weight;
            foreach (var val in axisValues)
            {
                result *= val;
            }
            return result;
        }
    }
}