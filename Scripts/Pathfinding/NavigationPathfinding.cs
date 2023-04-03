using System;
using System.Collections.Generic;
using UnityEngine;
using WalkableArea = NavigationZone.WalkableArea;
using AreaConnection = NavigationZone.AreaConnection;

public class NavigationPathfinding
{
    public  List<NavigationZone> NavigationZones;

    private static NavigationPathfinding instance = null;
    public static NavigationPathfinding Instance { 
        get
        {
            if (instance == null)
            {
                instance = new NavigationPathfinding();               
            }
            return instance;
        }
    }

    public NavigationPathfinding()
    {
        NavigationZones = new List<NavigationZone>();
        NavigationZones.AddRange(GameObject.FindObjectsOfType<NavigationZone>());
    }

    private double AreaCostFunction(AreaConnection connection)
    {
        return 1d;
    }

    public WalkableArea GetEnclosingArea(Vector3 point) {        
        Graph<WalkableArea, AreaConnection> areaGraph = null;
        foreach(var navzone in NavigationZones)
        {
            if (navzone.Contains(point))
            {
                areaGraph = navzone.AreaTopologyGraph;
                break;
            }
        }
        if (areaGraph == null) return null; //TODO: Maybe throw an exception

        Ray ray = new Ray(point, -Vector3.up);
        WalkableArea topmostArea = null;
        float topmostY = -Mathf.Infinity;

        foreach (var area in areaGraph.Vertices) //TODO: implement a faster method of search, maybe use a tree based one
        {
            if (area.WorldBounds.IntersectRay(ray)) 
            {
                if (area.WorldBounds.center.y > topmostY)
                {
                    topmostY = area.WorldBounds.center.y;
                    topmostArea = area;
                }                
            }
        }
        return topmostArea; //TODO: Either throw an exception if ray doesn't intersect with no area or select the closest visible area.
    }    

    public Vector3[] GetPath(Vector3 from, Vector3 to) {
        var startArea = GetEnclosingArea(from);
        var endArea = GetEnclosingArea(to);

        throw new NotImplementedException();
    }        
}
