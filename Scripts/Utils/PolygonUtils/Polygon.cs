using UnityEditor;
using UnityEngine;
using System.Linq;


public struct Segment {
    public Vector3 a;
    public Vector3 b;
    public float Length { 
        get 
        {
            return (b - a).magnitude;    
        } 
    }
    public Segment(Vector3 a, Vector3 b)
    {
        this.a = a;
        this.b = b;
    }
}

public struct Polygon
{
    public Vector3[] vertices { get; set; }

    private Segment[] _edges;

    public Segment[] Edges
    {
        get
        {
            if (_edges == null)
            {
                _edges = new Segment[vertices.Length - 1];
                for (int i = 0; i < vertices.Length - 1; i++)
                {
                    _edges[i].a = vertices[i];
                    _edges[i].a = vertices[i + 1];
                }
            }
            return _edges;
        }
    }
}
