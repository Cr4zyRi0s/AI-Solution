using System.Collections;
using UnityEngine;

namespace AI.Action.TargetSelection
{
    public class RandomPositionTargetSelector : BaseTargetSelector
    {
        private Vector3 _range = new Vector3(10,10,10);
        public Vector3 Range
        {
            get { return _range; }
            set { _range = value; }
        }

        public override ITarget Select(ActionContext context)
        {
            var currPosition = context.AgentPosition;
            var rand = new Vector3(
                currPosition.x + Random.Range(-_range.x, _range.x),
                currPosition.y + Random.Range(-_range.y, _range.y),
                currPosition.z + Random.Range(-_range.z, _range.z)
                );
            return new AnonymousTarget(rand);
        }
    }
}