
namespace AI.Action.TargetSelection {
    public abstract class BaseTargetSelector : ITargetSelector
    {
        public abstract ITarget Select(ActionContext context);
    }
}