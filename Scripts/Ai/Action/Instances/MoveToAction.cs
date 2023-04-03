using System;
using System.Collections;
using UnityEngine;
using AI.Action.TargetSelection;

namespace AI.Action
{
    public class MoveToAction : ActionBase
    {
        public override ActionState State
        {
            get => _state;
            set { _state = value; }
        }       
        public float ErrorTolerance { get; set; }
        public override float Cost
        {
            get
            {
                if (_actionContext.AIAgent.MovementSettings.MovementType == AgentMovementType.HUMANOID)
                    return  _actionContext.AgentDistanceFromStartToTarget(_agentStartPosition,_target.Position) 
                        / _actionContext.AgentWalkingSpeed;
                else
                    throw new Exception("Move to action not supported for non humanoid agents.");
            }
        }
        public override float RunningCost => _timeElapsed;
        public override bool SelfTargeted => false;

        private ITarget _target;
        private ActionState _state;
        private MovementActionEffect _moveEffect;

        private float _timeElapsed;
        private Vector3 _agentStartPosition;

        public MoveToAction(ActionContext context) : base(context) {}
        public override IEnumerable Execute(ActionSettings<string, object> settings = null) 
        {
            _agentStartPosition = _actionContext.AgentPosition;  
            _target = TargetSelector.Select(_actionContext);
            _moveEffect = new MovementActionEffect(_target.Position);
            EffectRequests.Add(_moveEffect);
            while (!_moveEffect.HasEnded)
            {
                _state = ActionState.RUNNING;
                yield return _state;
            }
            if (_moveEffect.Result.Error > ErrorTolerance)
            {
                _state = ActionState.FAILURE;
                yield return _state;
            }
            _state = ActionState.SUCCESS;
            yield return _state;
        }

        public override void Terminate()
        {
            if (_state == ActionState.RUNNING)
                _moveEffect.Cancel();
        }
    }
}