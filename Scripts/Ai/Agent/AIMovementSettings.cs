using System.Collections;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "AIMovementSettings", menuName = "AI/AIMovementSettings", order = 1)]
    public class AIMovementSettings : ScriptableObject
    {
        public AgentMovementType MovementType;
        public HumanoidAgentMovementSettings Humanoid;
    }

    public class HumanoidAgentMovementSettings
    {
        public bool CanJump;
        public float WalkingSpeed;
        public float RunningSpeed;
        public bool CanRun;
    }

    public enum AgentMovementType
    {
        HUMANOID = 0,
    }
}