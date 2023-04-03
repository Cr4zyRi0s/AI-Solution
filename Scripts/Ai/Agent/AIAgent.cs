using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Action;
using AI.Planner;

namespace AI
{
    public class AIAgent : MonoBehaviour, IPerceptionReceiver
    {
        public AIMovementSettings MovementSettings;
        public AIMovementGoalScriptableObject MovementGoal;
        public AIActionGoalScriptableObject ActionGoal;

        public ActionPlannerType PlannerType;
        public AgentNavigationSystemType NavigationType;

        [Range(0.0167f, 10f)]
        public float PlanningInterval = 0.5f;
        [Range(0.0167f, 10f)]
        public float EnvironmentPerceptionInterval = 0.33f;

        public IAgentNavigationSystem NavigationSystem { get; private set; }
        public IAgentMovement MovementController { get; private set; }
        public IActionProvider ActionProvider { get; private set; }
        public AgentBeliefState BeliefState { get; private set; }        
        public HashSet<ActionType> ActionSet { get; private set; }


        private Coroutine _planningCoroutine;
        private Coroutine _evaluatePerceptionCoroutine;
        private Stack<PerceptionData> _receivedPerceptions;

        public void ReceivePerception(PerceptionData perception)
        {
            _receivedPerceptions.Push(perception);
        }

        private IEnumerator EvaluateEnvironment()
        {
            var delay = new WaitForSeconds(EnvironmentPerceptionInterval);
            while (Application.isPlaying)
            {
                var perceptions = new List<PerceptionData>();

                foreach (var percept in _receivedPerceptions)
                {
                    perceptions.Add(percept);
                }

                //Formulate Belief State
                BeliefState.Elaborate(perceptions);
                _receivedPerceptions.Clear();
                yield return delay;
            }
        }

        private IEnumerator PlanAction()
        {
            var delay = new WaitForSeconds(PlanningInterval);
            while (Application.isPlaying)
            {
                //Plan Actions based on BeliefState
                yield return delay;
            }
        }

        private void Act()
        {
            var movVector = NavigationSystem.GetMovementVector();
            MovementController.Move(movVector);
        }

        //-------------------------------------------UNITY METHODS---------------------------------------------
        #region UNITY_METHODS

        private void Awake()
        {
            BeliefState = new AgentBeliefState();
            _receivedPerceptions = new Stack<PerceptionData>();
            NavigationSystem = AgentNavigationSystemFactory.GetNavigationSystem(NavigationType);

        }

        private void Start()
        {
            AgentPerceptionManager.Instance.SubscribeAgent(this);

            var movementControllers = GetComponentsInChildren<IAgentMovement>();
            Debug.Assert(movementControllers.Length == 1, "No IAgentMovement component found.");
            MovementController = movementControllers[0];
        }

        private void Update()
        {
            Act();
        }

        private void OnEnable()
        {
            _planningCoroutine = StartCoroutine(PlanAction());
            _evaluatePerceptionCoroutine = StartCoroutine(EvaluateEnvironment());
        }

        private void OnDisable()
        {
            StopCoroutine(_planningCoroutine);
            StopCoroutine(_evaluatePerceptionCoroutine);
        }
        #endregion
    }
}