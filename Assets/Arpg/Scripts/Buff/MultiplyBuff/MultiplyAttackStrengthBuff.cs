namespace Arpg.Scripts.Buff.MultiplyBuff
{
    /// <summary>
    /// 增加攻击力比率buff
    /// </summary>
    public class MultiplyAttackStrengthBuff:BaseBuff,IMultiplyBuff
    {
        private float multiplyRate;
        public MultiplyAttackStrengthBuff(AgentMonitor self,float multiplyRate) : base(self, null)
        {
            this.multiplyRate = multiplyRate;
        }

        public void DoBuff(System.Action callback)
        {
            self.ChangeMultiplyAttackRate(this.multiplyRate);
            callback();
        }

        public void UnDoBuff(System.Action callback)
        {
            self.ChangeMultiplyAttackRate(-this.multiplyRate);
            callback();
        }
    }
}