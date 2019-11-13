using System;
using Arpg.Action;
using Arpg.Scripts.Agent;

namespace Arpg.Condition
{
    public class BaseCondition
    {

        protected BaseAction fromAction, toAction1,toAction2,toAction3,toAction4;
        protected BaseAIGraph _graph;
        public BaseCondition(BaseAIGraph graph)
        {
            this._graph = graph;
        }

        public void SetTranslation(BaseAction fromAction,BaseAction toAction)
        {
            this.fromAction = fromAction;
            this.toAction1 = toAction;
        }

        public void SetTranslation(BaseAction fromAction,BaseAction toAction1,BaseAction toAction2)
        {
            this.fromAction = fromAction;
            this.toAction1 = toAction1;
            this.toAction2 = toAction2;
        }
        
        public void SetTranslation(BaseAction fromAction,BaseAction toAction1,BaseAction toAction2,BaseAction toAction3)
        {
            this.fromAction = fromAction;
            this.toAction1 = toAction1;
            this.toAction2 = toAction2;
            this.toAction3 = toAction3;
        }
        
        public void SetTranslation(BaseAction fromAction,BaseAction toAction1,BaseAction toAction2,BaseAction toAction3,BaseAction toAction4)
        {
            this.fromAction = fromAction;
            this.toAction1 = toAction1;
            this.toAction2 = toAction2;
            this.toAction3 = toAction3;
            this.toAction4 = toAction4;
        }

        public BaseAction GetNextAction(int id)
        {
            if (id == 1)
            {
                return toAction1;    
            }else if (id == 2)
            {
                return toAction2;
            }else if (id == 3)
            {
                return toAction3;
            }
            else
            {
                return toAction4;
            }
            
        }
    }
}