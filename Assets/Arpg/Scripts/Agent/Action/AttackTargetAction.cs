using Arpg.Action;
using Arpg.Scripts.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg.Agent.Action
{
    public class AttackTargetAction:BaseAction,IAction
    {
        private NavMeshAgent _navMeshAgent;
        public AttackTargetAction(BaseAIGraph aiGraph) : base(aiGraph)
        {
            _navMeshAgent = aiGraph.AgentMonitor.GetComponent<NavMeshAgent>();
        }

        public void Start()
        {
            _navMeshAgent.enabled = false;
            aiGraph.AgentMonitor.transform.forward = Vector3.forward;
            if (aiGraph.AgentMonitor.TargetEnemy != null && aiGraph.AgentMonitor.TargetEnemy.Alive == true)
            {
                var dir = aiGraph.AgentMonitor.TargetEnemy.transform.position -
                                                       aiGraph.AgentMonitor.transform.position;
                aiGraph.AgentMonitor._agentView.TryRunView(dir.normalized);
                aiGraph.TryAttack();    
            }
        }

        public void Update()
        {
            
        }

        public void Quit()
        {
           
        }
        
    }
}