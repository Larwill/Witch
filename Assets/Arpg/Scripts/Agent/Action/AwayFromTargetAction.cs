using System;
using Arpg.Action;
using Arpg.Scripts.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace Arpg.Agent.Action
{
    public class AwayFromTargetAction:BaseAction,IAction
    {
        private NavMeshAgent _navMeshAgent;
        private float minDst;
        private static bool moveAway = true;
        private float currentMoveAwayTime = 0;
        public AwayFromTargetAction(BaseAIGraph aiGraph,float beginAttackRadius) : base(aiGraph)
        {
            _navMeshAgent = aiGraph.AgentMonitor.GetComponent<NavMeshAgent>();
            this.minDst = beginAttackRadius;
        }

        public void Start()
        {
            _navMeshAgent.enabled = true;
            moveAway = true; 
            if (moveAway)
            {
                currentMoveAwayTime = 0;
                complete = false;
                var targetEnemy = aiGraph.AgentMonitor.TargetEnemy;
                if (targetEnemy != null && targetEnemy.Alive == true)
                {
                    var dir = targetEnemy.transform.position - aiGraph.AgentMonitor.transform.position;
                    if (Mathf.Abs(dir.x) < minDst)
                    {
                        aiGraph.AgentMonitor.TryAvoid(targetEnemy.transform.position,minDst);
                    }
                    else
                    {
                        complete = true;
                    }
                }
                else
                {
                    complete = true;
                } 
            }
            else
            {
                complete = true;
            }
            moveAway = !moveAway;
        }

        public void Update()
        {
            if (complete == false)
            {
                currentMoveAwayTime += Time.deltaTime;
                if (currentMoveAwayTime > 0.5f)
                {
                    complete = true;
                }
                var targetEnemy = aiGraph.AgentMonitor.TargetEnemy;
                if (targetEnemy != null && targetEnemy.Alive == true)
                {
                    var dir =  targetEnemy.transform.position - aiGraph.AgentMonitor.transform.position;
                    if (Mathf.Abs(dir.x) < minDst)
                    {
                        //aiGraph.AgentMonitor.TryAvoid(targetEnemy.transform.position,minDst);
                    }
                    else
                    {
                        complete = true;
                    }
                }
                else
                {
                    complete = true;
                }

            }
        }

        public void Quit()
        {
            
        }
    }
}