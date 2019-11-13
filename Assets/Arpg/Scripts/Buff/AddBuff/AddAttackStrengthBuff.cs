namespace Arpg.Scripts.Buff.AddBuff
{
    /// <summary>
    /// 增加攻击力buff
    /// </summary>
    public class AddAttackStrengthBuff :BaseBuff,IAddBuff
    {
        private int addOnAttackStrength;
        public AddAttackStrengthBuff(AgentMonitor self,int addOnAttackStrength) : base(self, null)
        {
            this.addOnAttackStrength = addOnAttackStrength;
        }

        public void DoBuff(System.Action callback)
        {
            for (int i = 0; i < self.baseAttackValues.Count; i++)
            {
                self.baseAttackValues[i] = self.baseAttackValues[i] + addOnAttackStrength;
            }
            callback();
        }

        public void UnDoBuff(System.Action callback)
        {
            for (int i = 0; i < self.baseAttackValues.Count; i++)
            {
                self.baseAttackValues[i] = self.baseAttackValues[i] - addOnAttackStrength;
            }
            callback();
        }
        
    }
}