using Arpg.Condition;
using Arpg.Scripts.Agent;

namespace Arpg.Agent.Condition
{
    /// <summary>
    /// 是否没有攻击到目标
    /// </summary>
    public class HasNotAttackedTargetCondition : BaseCondition,ICondition
    {
        public HasNotAttackedTargetCondition(BaseAIGraph graph) : base(graph)
        {
        }

        public int Check()
        {
            if (_graph.AgentMonitor.AttackOnTarget == null)
            {
                return 1;
            }

            return -1;
        }
        
    }
}