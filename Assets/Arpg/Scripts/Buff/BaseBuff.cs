namespace Arpg.Scripts.Buff
{
    public class BaseBuff
    {
        protected AgentMonitor self, target;
        public BaseBuff(AgentMonitor self,AgentMonitor target)
        {
            this.self = self;
            this.target = target;
        }
    }
}