
using UnityEngine;

public class BoxLOSChecker{     
    public static bool Check(Vector3 startPos, Bounds box)
    {
        var vertices = new Vector3[] {
            box.min + Vector3.right * box.size.x,
            box.min + Vector3.right * box.size.x,
            box.min + Vector3.forward * box.size.z,
            box.min + Vector3.right * box.size.x + Vector3.forward * box.size.z,
            box.max - Vector3.right * box.size.x,
            box.max - Vector3.right * box.size.x,
            box.max - Vector3.forward * box.size.z,
            box.max - Vector3.right * box.size.x - Vector3.forward * box.size.z,
        };

        RaycastHit hit;
        Vector3 endPos;
        foreach (var vertex in vertices)
        {
            endPos = vertex;
            if (!Physics.Raycast(startPos, endPos - startPos, out hit, (endPos - startPos).magnitude))
            {
                return true;
            }            
        }
        return false;
    }
}
