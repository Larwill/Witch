using Arpg.Action;
using Arpg.Scripts.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg.Agent.Action
{
    public class NavToTargetAction : BaseAction, IAction
    {
        private float closestX;

        public NavToTargetAction(BaseAIGraph aiGraph, float closestX) : base(aiGraph)
        {
            this.closestX = closestX;
        }

        public void Start()
        {
            _navMeshAgent.enabled = true;

            if (aiGraph.AgentMonitor.TargetEnemy != null)
            {
                aiGraph.AgentMonitor.TryNav(aiGraph.AgentMonitor.TargetEnemy.transform.position);
            }
        }

        public void Update()
        {
            var targetEnemy = aiGraph.AgentMonitor.TargetEnemy;
            if (targetEnemy != null)
            {
                aiGraph.AgentMonitor.TryNav(targetEnemy.transform.position);
            }
        }

        public void Quit()
        {
        }
    }
}