using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Action.TargetSelection;

namespace AI.Action
{
    public abstract class ActionBase : IAction
    {
        public abstract bool SelfTargeted { get; }
        public abstract ActionState State { get; set; }
        public abstract float Cost { get; }
        public abstract float RunningCost { get; }
        public ITargetSelector TargetSelector { get; set; }
        public List<IActionEffectRequest> EffectRequests { get; protected set; }

        protected HashSet<IActionEffectRequest> _pendingRequests;
        protected ActionContext _actionContext;

        public ActionBase(ActionContext context)
        {
            EffectRequests = new List<IActionEffectRequest>();
            _pendingRequests = new HashSet<IActionEffectRequest>();
            _actionContext = context;
        }
        public abstract IEnumerable Execute(ActionSettings<string, object> settings = null);
        public abstract void Terminate();
        public virtual IEnumerable<IActionEffectRequest> ConsumeRequests()
        {
            var list = EffectRequests.ToList();
            foreach (var effectRequest in list)
            {
                _pendingRequests.Add(effectRequest);
            }
            EffectRequests.Clear();
            return list;
        }
    }
}



//public virtual void SetResults(IEnumerable<IActionEffectResult> results)
//{
//    foreach(var result in results)
//    {
//        if (_pendingRequests.Contains(result.Request))
//            _pendingRequests.Remove(result.Request);
//        else
//            throw new Exception("Gave a result for an invalid Request");            
//    }
//    EffectResults.AddRange(results);
//}