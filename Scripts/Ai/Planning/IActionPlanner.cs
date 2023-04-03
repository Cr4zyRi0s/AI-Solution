using System.Collections.Generic;
using AI.Action;

namespace AI.Planner
{
    public interface IActionPlanner
    {
        public abstract IEnumerable<IAction> Plan();
    }

}