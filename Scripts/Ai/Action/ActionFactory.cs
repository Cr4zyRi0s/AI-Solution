using System;
namespace AI.Action
{
    public class ActionFactory
    {
        public static IAction GetAction(ActionContext actionContext, ActionType type)
        {
            switch (type)
            {
                case ActionType.MOVE_TO:
                    return new MoveToAction(actionContext);
                default:
                    throw new Exception();
            }
        }
    }
}