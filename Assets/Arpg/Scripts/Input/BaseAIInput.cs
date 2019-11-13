using System;
using Arpg.Scripts.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg
{
    [RequireComponent(typeof(AgentMonitor))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseAIInput:MonoBehaviour
    {
        private BaseAIGraph aiGraph;
        private AgentMonitor _agentMonitor;
        public float beginAttackRadius = 7;
        public float searchRadius = 50;
        public float lostRadius = 70;
        public float tooClosestX = 5;
        private void Awake()
        {
            _agentMonitor = this.GetComponent<AgentMonitor>();
            _agentMonitor.SetIsAIControl();
            _agentMonitor.updateAction = this.UpdateAction;
        }
        
        

        private void Start()
        {
            aiGraph = new BaseAIGraph(_agentMonitor);
            aiGraph.InitBaseLogic(searchRadius,beginAttackRadius,lostRadius,tooClosestX);
            aiGraph.StartGraph();
        }

         void UpdateAction()
        {
            aiGraph.UpdateGraph();
        }
        
        
    }
}