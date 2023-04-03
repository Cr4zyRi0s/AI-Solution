using UnityEditor;
using UnityEngine;
using System.Linq;

public static class PolygonUtils
{

    public static Vector3 ClosestLinePointToPoint(Segment line, Vector3 point) {
        var dir = line.b - line.a;
        var l = line.Length;
        var proj = Vector3.Dot(point,dir) / l;
        var t = Mathf.Clamp(proj / l, 0f, 1f);
        return line.a + t * (dir);
    }

    public static Vector3 FindClosestPolygonPointToPoint(Polygon poly, Vector3 point) {
        var edges = poly.Edges;

        var currDist = 0f;
        var currPoint = Vector3.zero;
        var minDist = Mathf.Infinity;
        var closestPoint = Vector3.zero;
        foreach (var edge in edges)
        {
            currPoint = ClosestLinePointToPoint(edge, point);
            currDist = Vector3.Distance(currPoint ,point);
            if(currDist < minDist)
            {
                minDist = currDist;
                closestPoint = currPoint;
            }
        }

        return closestPoint;
    }
}
