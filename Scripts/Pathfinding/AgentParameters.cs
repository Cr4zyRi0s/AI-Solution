using UnityEngine;

struct AgentParameters {
    public float height { get { return bounds.size.y; } }
    public Bounds bounds;
}