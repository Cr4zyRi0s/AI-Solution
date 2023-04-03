using UnityEngine;

namespace AI.Action
{
    public class ActionContext
    {
        public AIAgent @AIAgent { private set; get; }
        public AgentBeliefState @AgentBeliefState => AIAgent.BeliefState;
        public float AgentWalkingSpeed => AIAgent.MovementSettings.Humanoid.WalkingSpeed;
        public float AgentRunningSpeed => AIAgent.MovementSettings.Humanoid.RunningSpeed;
        public Vector3 AgentPosition => AIAgent.transform.position;
        public float AgentDistanceToTarget(Vector3 target) {
            return AIAgent.NavigationSystem.DistanceToTarget(target);
        }
        public float AgentDistanceFromStartToTarget(Vector3 start, Vector3 target)
        {
            return AIAgent.NavigationSystem.DistanceToTarget(target);
        }

        public ActionContext(AIAgent agent) 
        {
            this.AIAgent = agent;                
        }
    }
}