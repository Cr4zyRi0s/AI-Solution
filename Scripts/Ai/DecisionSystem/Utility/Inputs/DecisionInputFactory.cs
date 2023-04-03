using System;
namespace AI.DecisionSystem.Utility.Inputs {
    public class DecisionInputFactory {

        public static IDecisionInput GetDecisionInput(DecisionInputType type) {
            switch(type){
                case DecisionInputType.CONSTANT:
                    return new ConstantDecisionInput();
                case DecisionInputType.DISTANCE:
                    return new DistanceDecisionInput();
                case DecisionInputType.HEALTH:
                    return new HealthDecisionInput();              
                default:
                    throw new Exception("Got invalid DecisionInputType");            
            }
        }
    }
}