using System;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityConeChecker : IVisibilityChecker
{
    public Transform VisibilityTransform { get; private set; }    
    public float ConeAngle { get; private set; }


    public VisibilityConeChecker(Transform visibilityTransform, float coneAngle)
    {
        VisibilityTransform = visibilityTransform;
        ConeAngle = coneAngle;
    }

    public bool Check(Vector3 point)
    {
        if (!LOSChecker.Check(point, VisibilityTransform)) { return false; }
        if (Mathf.Abs(Vector3.Angle(VisibilityTransform.forward, point - VisibilityTransform.position)) > ConeAngle) { return false; }        
        return true;
    }

    public static bool InsideVisibilityCone(Vector3 point, Vector3 direction, float angle, Vector3 target)
    {
        return false;
    }

    public static bool InsideVisibilityCone(Vector3 point, Vector3 direction, float angle, Transform target)
    {
        return false;
    }
}
