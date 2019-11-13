using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arpg.EffectArea.StandardEffectArea
{
    public class AddAttackValueEffectArea:StandardAddOnEffectArea,IStandardAddOnEffectArea
    {
        public int addOnAttackValue = 100;
        public void EffectTarget(AgentMonitor agentMonitor)
        {
            agentMonitor.AddOnAttackValue(addOnAttackValue);
        }

        public void UnEffectTarget(AgentMonitor agentMonitor)
        {
            agentMonitor.AddOnAttackValue(-addOnAttackValue);
        }
    }
}