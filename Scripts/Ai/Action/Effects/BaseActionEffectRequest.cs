using System;
using System.Collections;
using UnityEngine;

namespace AI.Action
{
    public abstract class BaseActionEffectRequest : IActionEffectRequest
    {
        private IAction _senderAction;
        public IAction SenderAction => _senderAction;
        public bool HasEnded { protected set; get; }

        protected ActionEffectResult _result;
        public ActionEffectResult Result
        {
            get
            {
                if (!HasEnded)
                {
                    throw new Exception("Tried to read effect Result before completition.");
                }
                return _result;
            }
        }

        public BaseActionEffectRequest()
        {
            _result = new ActionEffectResult();
        }

        public virtual void OnEffectCompletition(AIAgent agent)
        {
            HasEnded = true;
        }

        public abstract void Cancel();        
    }
}