namespace Arpg.EffectArea.StandardEffectArea
{
    public class AddOnMoveSpeedEffectArea:StandardAddOnEffectArea,IStandardAddOnEffectArea
    {
        public float addOnMoveSpeed = 30;
        private float addOnRate = 0.5f;
        public void EffectTarget(AgentMonitor agentMonitor)
        {
            agentMonitor.AddMoveAndRunSpeed(addOnMoveSpeed);
            agentMonitor._agentView.ChangeRunSpeedRate(addOnRate);
        }

        public void UnEffectTarget(AgentMonitor agentMonitor)
        {
            agentMonitor.AddMoveAndRunSpeed(-addOnMoveSpeed);
            agentMonitor._agentView.ChangeRunSpeedRate(-addOnRate);
        }
    }
}