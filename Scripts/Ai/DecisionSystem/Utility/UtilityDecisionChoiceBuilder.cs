using System;
using System.Linq;
using System.Collections.Generic;
using AI.DecisionSystem.Utility.Inputs;
using AI.DecisionSystem.Utility.Scoring;

namespace AI.DecisionSystem.Utility
{
    public class UtilityDecisionChoiceBuilder
    {
        private HashSet<Axis> _axes;
        private IScoringStrategy _scorer;
        private double _weight = 1d;

        public UtilityDecisionChoiceBuilder()
        {
            _axes = new HashSet<Axis>();
        }

        public UtilityDecisionChoiceBuilder AddAxis(DecisionInputType inputType,
                                                    DecisionInputParams inputParams,
                                                    ResponseCurveType curveType, 
                                                    ResponseCurveParams curveParams)
        {

            var input = DecisionInputFactory.GetDecisionInput(inputType);
            input.Min = inputParams.Min;
            input.Max = inputParams.Max;

            var curve = ResponseCurveFactory.GetResponseCurve(curveType,
                                                            curveParams.Slope,
                                                            curveParams.Exponent,
                                                            curveParams.XOffset,
                                                            curveParams.YOffset);  
            _axes.Add(new Axis(input, curve));
            return this;
        }

        public UtilityDecisionChoiceBuilder AddAxis(Axis axis)
        { 
            _axes.Add(axis);
            return this;
        }

        public UtilityDecisionChoiceBuilder SetWeight(double weight)
        {
            _weight = weight;
            return this;
        }

        public UtilityDecisionChoiceBuilder SetScoringStrategy(ScoringStrategyType scorerType)
        {
            _scorer = ScoringStrategyFactory.GetScoringStrategy(scorerType);
            return this;
        }

        public UtilityDecisionChoiceBuilder SetScoringStrategy(IScoringStrategy scorer) {
            _scorer = scorer;
            return this;
        }

        public UtilityDecisionChoice Build()
        {
            if (_scorer == null) {
                throw new Exception("Scorer was not specified");
            }
            if (_axes.Count == 0) {
                throw new Exception("Axes were not specified");
            }
            return new UtilityDecisionChoice(_axes, _scorer, _weight);
        }
    }
}