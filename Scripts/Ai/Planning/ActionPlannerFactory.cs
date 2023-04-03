using System;

namespace AI.Planner
{
    public class ActionPlannerFactory
    {
        public static IActionPlanner GetActionPlanner(ActionPlannerType type)
        {
            switch (type)
            {
                case ActionPlannerType.NONE:
                    return null;
                default:
                    throw new Exception("Received unexpected ActionPlannerType.");
            }
        }
    }
}