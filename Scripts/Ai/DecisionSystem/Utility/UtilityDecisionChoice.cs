using System.Linq;
using System.Collections.Generic;
using AI.DecisionSystem.Utility.Scoring;

namespace AI.DecisionSystem.Utility {

    public class UtilityDecisionChoice : IDecisionChoice {
        public HashSet<Axis> Axes { get; private set; }

        public IScoringStrategy Scorer { get; private set; }

        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public UtilityDecisionChoice(IEnumerable<Axis> axes, IScoringStrategy scorer, double weight = 1d) {
            Axes = axes.ToHashSet();
            Weight = weight;
            Scorer = scorer;
        }
        public double Score
        {
            get {
                return Scorer.Score(Axes.Select(axis => axis.NormalizedValue), _weight);
            }
        }
    }
}