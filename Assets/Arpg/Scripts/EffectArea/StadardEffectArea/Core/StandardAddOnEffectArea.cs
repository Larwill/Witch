using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arpg.EffectArea.StandardEffectArea
{
    public class StandardAddOnEffectArea:StandardEffectArea
    {
        public float unEffectTime = 10f;
        public GameObject addOnVisualEffect;
        protected List<AgentMonitor> toUnEffectAgents;
        protected List<float> toUnEffectTimes;
        protected List<GameObject> addOnVisualEffects;
        private void Start()
        {
            toUnEffectAgents = new List<AgentMonitor>();
            toUnEffectTimes = new List<float>();
            addOnVisualEffects = new List<GameObject>();
            this.onAgentMonitorEnter = (agentMonitor) =>
            {
                if (toUnEffectAgents.Contains(agentMonitor))
                {
                    var index = toUnEffectAgents.IndexOf(agentMonitor);
                    toUnEffectAgents.RemoveAt(index);
                    toUnEffectTimes.RemoveAt(index);
                    return;
                }else
                {
                    GameObject newVisualEffect = GameObject.Instantiate(addOnVisualEffect);
                    var vetransform = newVisualEffect.transform;
                    vetransform.parent = agentMonitor.transform;
                    vetransform.localPosition = Vector3.zero;
                    vetransform.localScale = new Vector3(1,1,1);
                    newVisualEffect.SetActive(true);
                    addOnVisualEffects.Add(newVisualEffect);
                    ((IStandardAddOnEffectArea)this).EffectTarget(agentMonitor);                
                }
            };
            this.onAgentMonitorExit = (agentMonitor) =>
            {
                toUnEffectAgents.Add(agentMonitor);
                toUnEffectTimes.Add(unEffectTime);
            };
        }
        
        
//        private void EffectAgent(AgentMonitor agent)
//        {
//            agent.AddOnAttackValue(attackValue);
//        }
//
//        private void UnEffectAgent(AgentMonitor agent)
//        {
//            agent.AddOnAttackValue(-attackValue);
//        }
        
        private void Update()
        {
            for (int i = 0; i < toUnEffectTimes.Count; i++)
            {
                toUnEffectTimes[i] -= Time.deltaTime;
                if (toUnEffectTimes[i] < 0)
                {
                    toUnEffectTimes.RemoveAt(i);
                    var agent = toUnEffectAgents[i];
                    ((IStandardAddOnEffectArea)this).UnEffectTarget(agent);
                    toUnEffectAgents.RemoveAt(i);
                    var effect = addOnVisualEffects[i];
                    Destroy(effect); 
                    addOnVisualEffects.RemoveAt(i);
                }
            }
        }
    }
}