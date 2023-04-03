using UnityEngine;

namespace AI.Action.TargetSelection
{
    public interface ITarget
    {
        public abstract Vector3 Position { get; }
    }
}