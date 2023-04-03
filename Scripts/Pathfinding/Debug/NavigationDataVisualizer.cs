using VisualDebugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Layers { 
    VISIBILITY,
    COVER
}

public class NavigationDataVisualizer : MonoBehaviour
{
    public NavigationZone NavZone;
    public Transform Handle;
    public Layers ShownLayer;

    void Start()
    {        
        StartCoroutine(VisualizeData());
    }

    private IEnumerator VisualizeData()
    {        
        while (Application.isPlaying)
        {
            VisualDebugger.ClearAll(this);
            var sample = NavZone.NavData.GetKNearestSamples(Handle.position, 1)[0];

            switch (ShownLayer) {
                case Layers.VISIBILITY:
                    var data = NavZone.NavData.HeadLevelVisibilityLayer[sample];
                    foreach (var other in NavZone.NavData.Samples)
                    {
                        var point = VisualDebugger.AddPoint(this, other.position);
                        point.color = (data[other.id]) ? Color.green : Color.red;
                    };
                    break;
                case Layers.COVER:
                    foreach (var other in NavZone.NavData.Samples)
                    {
                        var point = VisualDebugger.AddPoint(this, other.position);
                        point.color = Color.Lerp(Color.red, Color.green, this.NavZone.NavData.CoverLayer[other]);
                    };
                    break;

            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDrawGizmos()
    {
        VisualDebugger.Render(this);
    }
}
