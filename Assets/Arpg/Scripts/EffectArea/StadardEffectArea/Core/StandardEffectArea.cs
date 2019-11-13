using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arpg.EffectArea.StandardEffectArea
{
    public class StandardEffectArea :MonoBehaviour
    {
        public List<int> effectLayers;
        protected Action<AgentMonitor> onAgentMonitorEnter;
        protected Action<AgentMonitor> onAgentMonitorExit;
        
        private void OnTriggerEnter(Collider other)
        {
            var agentMonitor = other.GetComponent<AgentMonitor>();
            if (agentMonitor == null)
            {
                return;
            }
            if (effectLayers.Contains(agentMonitor.gameObject.layer))
            {

                    if (onAgentMonitorEnter != null)
                    {
                        onAgentMonitorEnter(agentMonitor);
                    }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var agentMonitor = other.GetComponent<AgentMonitor>();
            if (agentMonitor == null)
            {
                return;
            }
            if (effectLayers.Contains(agentMonitor.gameObject.layer))
            {
                if (onAgentMonitorExit != null)
                {
                    onAgentMonitorExit(agentMonitor);
                }
            }
        }
        
    }
}