using System.Collections.Generic;
using UnityEngine;


public class NavigationZoneManager : MonoBehaviour
{

    private static NavigationZoneManager _instance;
    public static NavigationZoneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("_NAVIGATION_ZONE_MANAGER");
                _instance = obj.AddComponent<NavigationZoneManager>();
            }
            return _instance;
        }
    }

    public HashSet<NavigationZone> NavigationZones { get; private set; }


    private void Awake()
    {
        NavigationZones = new HashSet<NavigationZone>();
    }

    public NavigationZone GetEnclosingNavigationZone(Vector3 point)
    {
        foreach (var zone in NavigationZones)
        {
            if (zone.Contains(point))
                return zone;
        }
        return null;
    }

    public void SubscribeNavigationZone(NavigationZone zone) {
        this.NavigationZones.Add(zone);
    }
}
