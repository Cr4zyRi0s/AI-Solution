using System;
using System.Collections.Generic;
using AI.DecisionSystem.Utility.Selection;

namespace AI.DecisionSystem.Utility
{
    public class UtilityDecisionSystemBuilder
    {        
        private List<UtilityDecisionChoice> _choices;
        private List<object> _objects;
        private ISelectionStrategy _selector;
        private UtilityDecisionChoiceBuilder _activeDecisionBuilder;

        public UtilityDecisionSystemBuilder AddChoice(UtilityDecisionChoice choice, object obj)
        {
            _choices.Add(choice);
            _objects.Add(obj);
            return this;
        }


        public UtilityDecisionSystemBuilder SetSelectionStrategy(ISelectionStrategy selector) {
            _selector = selector;
            return this;
        }

        public UtilityDecisionSystem Build()
        {
            if (_choices.Count == 0)
            {
                throw new Exception();
            }
            if (_objects.Count == 0)
            {
                throw new Exception();
            }
            if (_choices.Count != _objects.Count)
            {
                throw new Exception();
            }
            if (_selector == null) 
            {
                throw new Exception();
            }

            var sys = new UtilityDecisionSystem(_choices,_objects,_selector);
            return sys;
        }
    }
}