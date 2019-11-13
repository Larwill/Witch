
using Arpg.Condition;
using Arpg.Scripts.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg.Agent.Condition
{
    /// <summary>
    ///1. 继续攻击
    /// 还是
    ///2. 跟随目标
    /// 还是
    ///3. 重新搜索目标
    /// 判定
    /// </summary>
    public class AttackOrFollowOrResearchCondition : BaseCondition,ICondition
    {
        private float beginAttackRadius, lostRadius,tooClosestX;
        private NavMeshAgent _navMeshAgent;
        
        public AttackOrFollowOrResearchCondition(BaseAIGraph graph,float beginAttackRadius, float lostRadius,float tooClosestX) : base(graph)
        {
            this.beginAttackRadius = beginAttackRadius;
            this.lostRadius = lostRadius;
            this._navMeshAgent = graph.AgentMonitor.GetComponent<NavMeshAgent>();
            this.tooClosestX = tooClosestX;
        }

        public int Check()
        {
            if (_graph.AgentMonitor.TargetEnemy == null ||  _graph.AgentMonitor.TargetEnemy.Alive == false )
            {
                return 3;
            }
            if (_graph.AgentMonitor.AttackOnTarget == null)
            {
                return -1;
            }
            
            _navMeshAgent.stoppingDistance = this.beginAttackRadius;
            var lastDest = _navMeshAgent.destination;
            var dir = _graph.AgentMonitor.transform.position -
                      _graph.AgentMonitor.TargetEnemy.transform.position;
            var sqrDst = Vector3.SqrMagnitude(dir);
            if (Mathf.Abs(dir.x) < tooClosestX )
            {
                return 4;
            }
            if ( sqrDst <= _navMeshAgent.stoppingDistance*_navMeshAgent.stoppingDistance)
            {
                return 1;
            }else if (sqrDst > lostRadius*lostRadius)
            {
                _graph.AgentMonitor.TargetEnemy = null;
                return 3;
            }else if (sqrDst > this.beginAttackRadius*this.beginAttackRadius)
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.destination = _graph.AgentMonitor.TargetEnemy.transform.position;
                return 2;
            }
            
            return -1;
        }
    }
}