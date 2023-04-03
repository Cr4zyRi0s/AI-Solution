using UnityEngine;

public struct PerceptionData
{
    public double Timestamp;
    public Vector3 Position;
    public PerceptionType Type;

    public PerceptionData(Vector3 position, PerceptionType type)
    {
        Timestamp = Time.time;
        Position = position;
        Type = type;
    }
}
