using System.Linq;
using System.Collections.Generic;
using AI.DecisionSystem.Utility.Selection;

namespace AI.DecisionSystem.Utility
{
    public class UtilityDecisionSystem
    {
        public UtilityDecisionSystem(
            IEnumerable<UtilityDecisionChoice> choiceSet,
            IEnumerable<object> objectSet,
            ISelectionStrategy selector,
            string name = null)
        {            
            _name = name;
            _choicesToObjects = new Dictionary<UtilityDecisionChoice, object>();

            Choices = choiceSet.ToHashSet();
            Selector = selector;

            var choiceArray = choiceSet.ToArray();
            var objectArray = objectSet.ToArray();

            for (int i = 0; i < choiceArray.Length; i++)
            {
                _choicesToObjects.Add(choiceArray[i],objectArray[i]);
            }
        }

        private string _name;
        public string Name => _name ?? GetType().Name;
        public ISelectionStrategy Selector{ get; }
        public HashSet<UtilityDecisionChoice> Choices { get; }
        private Dictionary<UtilityDecisionChoice, object> _choicesToObjects;
        public object Selection => Selector.Select(Choices);
    }
}