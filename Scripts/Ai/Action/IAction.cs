using System.Collections;
using System.Collections.Generic;
using AI.Action.TargetSelection;
namespace AI.Action
{
    public interface IAction
    {
        public abstract bool SelfTargeted { get; }
        public abstract ActionState State { get; set; }
        public abstract ITargetSelector TargetSelector { get; }
        public abstract IEnumerable Execute(ActionSettings<string, object> settings = null);
        public abstract void Terminate();
        public abstract IEnumerable<IActionEffectRequest> ConsumeRequests();
    }
}