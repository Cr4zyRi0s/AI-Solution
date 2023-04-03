using AI.Action;

namespace AI.Action.TargetSelection
{
    public interface ITargetSelector
    {
        public ITarget Select(ActionContext context);
    }
}