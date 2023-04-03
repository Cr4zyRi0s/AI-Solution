using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AgentPerceptionManager : MonoBehaviour, IPerceptionProvider
    {
        private static AgentPerceptionManager _instance;
        public static AgentPerceptionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("_AGENT_PERCEPTION_MANAGER");
                    _instance = obj.AddComponent<AgentPerceptionManager>();
                }
                return _instance;
            }
        }

        public HashSet<AIAgent> Agents;
        public HashSet<IPerceptionSource> PerceptionSources;

        void Awake()
        {
            Agents = new HashSet<AIAgent>();
            PerceptionSources = new HashSet<IPerceptionSource>();
        }

        void Update()
        {

        }

        public void SubscribeAgent(AIAgent agent)
        {
            Agents.Add(agent);
        }

        public void SubscribePerceptionSource(IPerceptionSource source)
        {
            PerceptionSources.Add(source);
        }
    }
}