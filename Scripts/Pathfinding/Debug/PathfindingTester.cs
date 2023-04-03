using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WalkableArea = NavigationZone.WalkableArea;

public class PathfindingTester : MonoBehaviour
{
    public NavigationZone NavZone;
    public Transform Source;
    public Transform Target;

    private WalkableArea enclosingSource;
    private WalkableArea enclosingTarget;
    private Vector3 sourceLastPos;
    private Vector3 targetLastPos;

    private List<List<WalkableArea>> allPaths;

    private void Start()
    {
        enclosingSource = NavigationPathfinding.Instance.GetEnclosingArea(Source.position);
        enclosingTarget = NavigationPathfinding.Instance.GetEnclosingArea(Target.position);
        allPaths = NavZone.AreaTopologyGraph.AllPathsUndirected(enclosingSource, enclosingTarget);
        sourceLastPos = Source.position;
        targetLastPos = Target.position;
    }

    private void Update()
    {
        if (Vector3.Distance(sourceLastPos, Source.position) > 0.01f || Vector3.Distance(targetLastPos, Target.position) > 0.01f)
        {
            enclosingSource = NavigationPathfinding.Instance.GetEnclosingArea(Source.position);
            enclosingTarget = NavigationPathfinding.Instance.GetEnclosingArea(Target.position);
            allPaths = NavZone.AreaTopologyGraph.AllPathsUndirected(enclosingSource, enclosingTarget);
        }
        sourceLastPos = Source.position;
        targetLastPos = Target.position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(enclosingSource.WorldBounds.center, enclosingSource.WorldBounds.size);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(enclosingTarget.WorldBounds.center, enclosingTarget.WorldBounds.size);

            Gizmos.color = Color.yellow;
            foreach (var path in allPaths)
            {
                foreach (var area in path)
                {
                    Gizmos.DrawWireCube(area.WorldBounds.center, area.WorldBounds.size);
                }
            }
        }
    }
}
