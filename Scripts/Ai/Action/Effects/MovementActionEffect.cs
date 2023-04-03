using System;
using System.Collections;
using UnityEngine;

namespace AI.Action
{
    public class MovementActionEffect : BaseActionEffectRequest
    {
        public Vector3 Target { get; private set; }

        public MovementActionEffect(Vector3 target)
        {
            Target = target;
        }

        public override void OnEffectCompletition(AIAgent agent)
        {
            base.OnEffectCompletition(agent);
            _result.Error = Vector3.Distance(Target, agent.transform.position);
        }

        public override void Cancel()
        {
        }
    }
}