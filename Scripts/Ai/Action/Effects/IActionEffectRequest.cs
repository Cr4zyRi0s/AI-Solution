using System.Collections;
using UnityEngine;

namespace AI.Action
{
    public interface IActionEffectRequest
    {
        public IAction SenderAction { get; }
        public void OnEffectCompletition(AIAgent agent);
        public void Cancel();
    }
}