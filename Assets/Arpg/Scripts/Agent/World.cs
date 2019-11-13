using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arpg.Scripts.Agent
{
    public class World:MonoBehaviour
    {
        private List<AgentMonitor> agents;
        public static World instance;
        public Vector3 gravity;
        
        public void Awake()
        {
            agents = new List<AgentMonitor>();
            instance = this;
        }

        private void Update()
        {
            Physics.gravity = gravity;
        }

        /// <summary>
        /// 获取所有敌人
        /// </summary>
        /// <param name="enemyLayers"></param>
        /// <returns></returns>
        public List<AgentMonitor> GetEnemys(List<int> enemyLayers)
        {
                List<AgentMonitor> enemys = new List<AgentMonitor>();
                for (int i = 0; i < agents.Count; i++)
                {
                    var agent = agents[i];
                    var layer = agents[i].gameObject.layer;
                    if (enemyLayers.Contains(layer))
                    {
                        enemys.Add(agent);
                    }
                }
                return enemys;
        }

        /// <summary>
        /// 获取所有的队友
        /// </summary>
        /// <param name="friendLayers"></param>
        /// <returns></returns>
        public List<AgentMonitor> GetFriends(List<int> friendLayers)
        {
            List<AgentMonitor> friends = new List<AgentMonitor>();
            for (int i = 0; i < agents.Count; i++)
            {
                var agent = agents[i];
                var layer = agents[i].gameObject.layer;
                if (friendLayers.Contains(layer))
                {
                    friends.Add(agent);
                }
            }
            return friends;
        }
        
        
        
    }
}