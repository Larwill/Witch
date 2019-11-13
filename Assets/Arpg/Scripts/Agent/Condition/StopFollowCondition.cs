using System.Collections.Generic;
using Arpg.Condition;
using Arpg.Scripts.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg.Agent.Condition
{
    /// <summary>
    /// 停止追踪判定
    /// 1.攻击目标
    /// 2.跟随目标
    /// 3.idle
    /// </summary>
    public class StopFollowCondition:BaseCondition,ICondition
    {
        private float sqrBeginAttackRadius;
        private NavMeshAgent _navMeshAgent;
        private float maxReSeachTime = 1f;
        private float currentResearchTime = 0;
        private float searchTime = 0;
        private float maxSearchTime = 0.2f;
        private float searchRadius = 10;
        
        public StopFollowCondition(BaseAIGraph graph,float beginAttackRadius) : base(graph)
        {
            this.sqrBeginAttackRadius = beginAttackRadius;
            _navMeshAgent = graph.AgentMonitor.GetComponent<NavMeshAgent>();
            currentResearchTime = 0;
        }

        public int Check()
        {
            currentResearchTime += Time.deltaTime;
            if (currentResearchTime > maxReSeachTime)
            {
                currentResearchTime = 0f; 
                _graph.AgentMonitor.TargetEnemy = Search();
                if (_graph.AgentMonitor.TargetEnemy == null)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
            }
            
            _navMeshAgent.stoppingDistance = this.sqrBeginAttackRadius;
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                return 1;
            }
            return -1;
        }
        
        private AgentMonitor Search()
        {
            var self = this._graph.AgentMonitor;
            var radius = this.searchRadius;
            var orign = self.transform.position;
            var direction = self.transform.forward;
            var raycasthits = Physics.SphereCastAll(orign,radius,direction);
            List<GameObject> targets = new List<GameObject>();
            foreach (RaycastHit raycasthit in raycasthits)
            {
                var target = raycasthit.collider.gameObject;
                var agent = target.GetComponent<AgentMonitor>();
                var layer = target.layer;
                if (this._graph.AgentMonitor.enemyLayers.Contains(layer) && agent!= null && agent.Alive == true)
                {
                    targets.Add(target);
                }
            }

            GameObject goal = null;
            float minDst = float.MaxValue;
            foreach (GameObject gameObject in targets)
            {
                float curDst = Vector3.SqrMagnitude(self.transform.position - gameObject.transform.position);
                if ( curDst < minDst)
                {
                    goal = gameObject;
                    minDst = curDst;
                }
            }

            if (goal != null)
            {
                return goal.GetComponent<AgentMonitor>();
            }

            return null;
        }

    }
}