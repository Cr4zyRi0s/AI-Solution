using CustomAiNavigation.Zone.Data;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AIMovementGoal",menuName = "CustomAI/AIMovementGoal",order = 1)]
public class AIMovementGoalScriptableObject : ScriptableObject
{
    public float VisibilityWeight = 1f;
    public float CoverWeight = 1f;

    public float ValuePosition(NavigationData navData, Vector3 position, Vector3? otherPosition = null)  
    {
        float score = 0f;
        var positionNearestSample = navData.GetKNearestSamples(position, 1)[0];

        NavigationSample otherNearestSample = null;
        if (otherPosition != null)
            otherNearestSample = navData.GetKNearestSamples(otherPosition.Value, 1)[0];
        if (VisibilityWeight != 0f) {                        
            score += (navData.HeadLevelVisibilityLayer[positionNearestSample, otherNearestSample]) ? VisibilityWeight : 0;
        }
        if (CoverWeight != 0f)
        {
            score += navData.CoverLayer[positionNearestSample] * CoverWeight;
        }
        return score;
    }
}