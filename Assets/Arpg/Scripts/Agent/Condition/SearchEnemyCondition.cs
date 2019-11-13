using System.Collections.Generic;
using Arpg.Condition;
using Arpg.Scripts.Agent;
using UnityEngine;

namespace Arpg.Agent.Condition
{
    public class SearchEnemyCondition : BaseCondition,ICondition
    {
        private float searchTime = 0;
        private float maxSearchTime = 0.2f;
        private float searchRadius = 10;
        public int Check()
        {
            if (this._graph.AgentMonitor.TargetEnemy != null && this._graph.AgentMonitor.TargetEnemy.Alive == true)
            {
                return -1;
            }

            this._graph.AgentMonitor.TargetEnemy = null;
            searchTime += Time.deltaTime;
            if (searchTime > maxSearchTime)
            {
                searchTime -= maxSearchTime;
                var searchTarget = Search();
                if (searchTarget != null)
                {
                    this._graph.AgentMonitor.TargetEnemy = searchTarget;
                    return 1;
                }
                else
                {
                    return 2;
                }
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


        public SearchEnemyCondition(BaseAIGraph graph,float radius) : base(graph)
        {
            this.searchRadius = radius;
        }
    }
}