using System.Collections;
using UnityEngine;


public abstract class AgentNavigationSystemBase : IAgentNavigationSystem
{
    protected Vector3 _destination;
    public virtual Vector3 Destination { set => _destination = value; }
    public abstract float DistanceToTarget(Vector3 target);
    public abstract float DistanceFromStartToTarget(Vector3 start, Vector3 target);
    public abstract Vector3 GetMovementVector();    
}
