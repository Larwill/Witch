using System.Collections.Generic;
using Arpg.Condition;
using Arpg.Scripts.Agent;
using UnityEngine.AI;

namespace Arpg.Action
{
    public class BaseAction
    {
        protected NavMeshAgent _navMeshAgent;
        public List<BaseCondition> conditions;
        protected BaseAIGraph aiGraph;
        public bool complete = false;
        
        public BaseAction(BaseAIGraph aiGraph)
        {
            this.conditions = new List<BaseCondition>();
            this.aiGraph = aiGraph;
            this._navMeshAgent = this.aiGraph.AgentMonitor.GetComponent<NavMeshAgent>();
        }

        public void AddCondition(BaseCondition condition)
        {
            this.conditions.Add(condition);
        }
        
    }
}