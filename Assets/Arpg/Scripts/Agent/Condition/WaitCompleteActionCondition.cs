using Arpg.Condition;
using Arpg.Scripts.Agent;

namespace Arpg.Agent.Condition
{
    public class WaitCompleteActionCondition:BaseCondition,ICondition
    {
        public WaitCompleteActionCondition(BaseAIGraph graph) : base(graph)
        {
        }

        public int Check()
        {
            if (fromAction.complete == true)
            {
                return 1;
            }

            return -1;
        }
    }
}