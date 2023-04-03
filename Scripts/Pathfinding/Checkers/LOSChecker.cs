using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LOSChecker
{
    public static bool Check(Vector3 source, Vector3 target)
    {
        var ray = new Ray();
        ray.origin = source;
        ray.direction = target - source;
        RaycastHit rhit;
        if (!Physics.Raycast(ray, out rhit, Vector3.Distance(source, target)))
        {
            return true;
        }        
        return false;
    }

    public static bool Check(Vector3 source, GameObject target)
    {
        return LOSChecker.Check(source, target.transform);
    }

    public static bool Check(Vector3 source, Transform target)
    {
        var ray = new Ray();
        ray.origin = source;
        ray.direction = target.position - source;
        RaycastHit rhit;
        if (!Physics.Raycast(ray, out rhit, Vector3.Distance(source, target.position)))
        {
            return true;
        }
        else
        {
            if (rhit.collider.transform.Equals(target)) { return true; }
        }
        return false;
    }
}
