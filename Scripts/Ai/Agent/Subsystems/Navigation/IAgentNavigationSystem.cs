using UnityEngine;
public interface IAgentNavigationSystem {    
    public Vector3 Destination { set; }
    public Vector3 GetMovementVector();
    public float DistanceToTarget(Vector3 target);
    public float DistanceFromStartToTarget(Vector3 start, Vector3 target);
}