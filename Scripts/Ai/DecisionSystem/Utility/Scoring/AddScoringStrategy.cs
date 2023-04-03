using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.DecisionSystem.Utility.Scoring
{
    public class AddScoringStrategy : IScoringStrategy
    {
        public double Score(IEnumerable<double> axisValues, double weight = 1)
        {
            var sum = axisValues.Sum();
            return sum * weight;
        }
    }
}