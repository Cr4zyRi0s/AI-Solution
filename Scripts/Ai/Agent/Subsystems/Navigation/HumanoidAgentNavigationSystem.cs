using System.Collections;
using UnityEngine;

public class HumanoidAgentNavigationSystem : AgentNavigationSystemBase
{
    public override float DistanceFromStartToTarget(Vector3 start, Vector3 target)
    {
        throw new System.NotImplementedException();
    }
    public override float DistanceToTarget(Vector3 position)
    {
        throw new System.NotImplementedException();
    }
    public override Vector3 GetMovementVector()
    {
        return Vector3.zero;
    }
}
