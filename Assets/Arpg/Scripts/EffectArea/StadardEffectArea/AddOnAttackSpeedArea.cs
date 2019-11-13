using UnityEditor;

namespace Arpg.EffectArea.StandardEffectArea
{
    public class AddOnAttackSpeedArea:StandardAddOnEffectArea,IStandardAddOnEffectArea
    {
        public float addOnAttackSpeedRate = 0.5f;
        
        public void EffectTarget(AgentMonitor agentMonitor)
        {
            agentMonitor.AddOnAttackSpeedRate(addOnAttackSpeedRate);
            agentMonitor._agentView.ChangeAttackSpeedRate(addOnAttackSpeedRate);
        }

        public void UnEffectTarget(AgentMonitor agentMonitor)
        {
            agentMonitor.AddOnAttackSpeedRate(-addOnAttackSpeedRate);
            agentMonitor._agentView.ChangeAttackSpeedRate(-addOnAttackSpeedRate);
        }
        
    }
}