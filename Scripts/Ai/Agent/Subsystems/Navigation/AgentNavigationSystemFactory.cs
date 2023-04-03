using System;

public class AgentNavigationSystemFactory{
    public static IAgentNavigationSystem GetNavigationSystem(AgentNavigationSystemType type) {
        switch (type)
        {
            case AgentNavigationSystemType.HUMANOID:
                return new HumanoidAgentNavigationSystem();
            default:
                throw new Exception("Received unexpected AgentMovementSystemType.");
        }
    }
}