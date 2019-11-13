namespace Arpg.Scripts.Buff.BeAttackBuff
{
    /// <summary>
    /// 反伤buff
    /// </summary>
    public class AntiAttackBuff:BaseBuff,IBeAttackBuff
    {
        private float addOnAntiAttackRate = 0f;
        public AntiAttackBuff(AgentMonitor self,float antiAttackRate) : base(self,null)
        {
            this.addOnAntiAttackRate = antiAttackRate;
        }

        public void DoBuff(System.Action callback)
        {
            self.ChangeAntiAttackRate(this.addOnAntiAttackRate);
            callback();
        }

        public void UnDoBuff(System.Action callback)
        {
            self.ChangeAntiAttackRate(-this.addOnAntiAttackRate);
            callback();
        }
        
    }
}