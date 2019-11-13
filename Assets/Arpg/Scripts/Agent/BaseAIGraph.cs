using System.Collections.Generic;
using Arpg.Action;
using Arpg.Agent.Action;
using Arpg.Agent.Condition;
using Arpg.Condition;

namespace Arpg.Scripts.Agent
{
    public class BaseAIGraph
    {
        private List<BaseAction> _actions;
        private AnyAction _anyAction;
        private BaseAction currentAction;
        private AgentMonitor _agentMonitor;

        public AgentMonitor AgentMonitor
        {
            get => _agentMonitor;
        }
        public BaseAIGraph(AgentMonitor agentMonitor)
        {
            this._agentMonitor = agentMonitor;
            this._actions = new List<BaseAction>();
            this._anyAction = new AnyAction(this);
        }

        public void AddAction(BaseAction action)
        {
            this._actions.Add(action);
        }
        public AnyAction GetAnyAction()
        {
            return this._anyAction;
        }

        public void SetStartAction(BaseAction startAction)
        {
            this.currentAction = startAction;
        }

        public void SetTranslation(BaseAction fromAction,BaseCondition condition,BaseAction toAction)
        {
            if (!this._actions.Contains(fromAction))
            {
                this._actions.Add(fromAction);
            }

            if (!this._actions.Contains(toAction))
            {
                this._actions.Add(toAction);
            }

            fromAction.AddCondition(condition);
            condition.SetTranslation(fromAction,toAction);
        }
       
        public void SetTranslation(BaseAction fromAction,BaseCondition condition,BaseAction toAction1,BaseAction toAction2)
        {
            if (!this._actions.Contains(fromAction))
            {
                this._actions.Add(fromAction);
            }

            if (!this._actions.Contains(toAction1))
            {
                this._actions.Add(toAction1);
            }

            if (!this._actions.Contains(toAction2))
            {
                this._actions.Add(toAction2);
            }
            
            fromAction.AddCondition(condition);
            condition.SetTranslation(fromAction,toAction1,toAction2);
        }

        public void SetTranslation(BaseAction fromAction,BaseCondition condition,BaseAction toAction1,BaseAction toAction2,BaseAction toAction3)
        {
            if (!this._actions.Contains(fromAction))
            {
                this._actions.Add(fromAction);
            }

            if (!this._actions.Contains(toAction1))
            {
                this._actions.Add(toAction1);
            }

            if (!this._actions.Contains(toAction2))
            {
                this._actions.Add(toAction2);
            }
            
            if (!this._actions.Contains(toAction3))
            {
                this._actions.Add(toAction3);
            }
            
            fromAction.AddCondition(condition);
            condition.SetTranslation(fromAction,toAction1,toAction2,toAction3);
        }
        

        public void SetTranslation(BaseAction fromAction,BaseCondition condition,BaseAction toAction1,BaseAction toAction2,BaseAction toAction3,BaseAction toAction4)
        {
            if (!this._actions.Contains(fromAction))
            {
                this._actions.Add(fromAction);
            }

            if (!this._actions.Contains(toAction1))
            {
                this._actions.Add(toAction1);
            }

            if (!this._actions.Contains(toAction2))
            {
                this._actions.Add(toAction2);
            }
            
            if (!this._actions.Contains(toAction3))
            {
                this._actions.Add(toAction3);
            }

            if (!this._actions.Contains(toAction4))
            {
                this._actions.Add(toAction4);
            }
            
            fromAction.AddCondition(condition);
            condition.SetTranslation(fromAction,toAction1,toAction2,toAction3,toAction4);
        }

        
        public void InitBaseLogic(float searchRadius,float beginattackRadius,float lostRadius,float tooClosestX)
        {
            IdleAction idleAction = new IdleAction(this);
            this.AddAction(idleAction);
            NavToTargetAction navToTargetAction = new NavToTargetAction(this,beginattackRadius);
            this.AddAction(navToTargetAction);
            AttackTargetAction attackTargetAction = new AttackTargetAction(this);
            this.AddAction(attackTargetAction);
            AwayFromTargetAction awayFromTargetAction = new AwayFromTargetAction(this,beginattackRadius*0.5f);
            this.AddAction(awayFromTargetAction);
            
            
            SearchEnemyCondition searchEnemyCondition = new SearchEnemyCondition(this,searchRadius);
            StopFollowCondition stopFollowCondition = new StopFollowCondition(this,beginattackRadius); 
            AttackOrFollowOrResearchCondition attackOrFollowOrResearchCondition = new AttackOrFollowOrResearchCondition(this,beginattackRadius,lostRadius,tooClosestX);
            WaitCompleteActionCondition waitCompleteActionCondition = new WaitCompleteActionCondition(this);
            HasNotAttackedTargetCondition hasNotAttackedTargetCondition = new HasNotAttackedTargetCondition(this);
            
            this.SetTranslation(_anyAction,searchEnemyCondition,navToTargetAction,idleAction);
            this.SetTranslation(navToTargetAction,stopFollowCondition,attackTargetAction,navToTargetAction,idleAction);
            this.SetTranslation(attackTargetAction,hasNotAttackedTargetCondition,awayFromTargetAction);
            this.SetTranslation(attackTargetAction,attackOrFollowOrResearchCondition,attackTargetAction,navToTargetAction,idleAction,awayFromTargetAction);
            this.SetTranslation(awayFromTargetAction,waitCompleteActionCondition,navToTargetAction);
            
            
            SetStartAction(idleAction);
        }
        
        public void StartGraph()
        {
            ((IAction) this.currentAction).Start();
        }

        private void ChangeAction(BaseAction newAction)
        {
            if (currentAction != null)
            {
                ((IAction)currentAction).Quit();
            }
            currentAction = newAction;
            ((IAction)currentAction).Start();
        }

        public void UpdateGraph()
        {
            if (_agentMonitor.CanRun == false)
            {
                return;
            }
            
            ((IAction)currentAction).Update();
            foreach (var condition in _anyAction.conditions)
            {
                var id = ((ICondition) condition).Check();
                if (id != -1)
                {
                    ChangeAction(condition.GetNextAction(id));
                    return;
                }
            }
            foreach (var condition in currentAction.conditions)
            {
                var id = ((ICondition) condition).Check();
                if (id != -1)
                {
                    ChangeAction(condition.GetNextAction(id));
                    return;
                }
            }
        }

        /// <summary>
        /// 尝试空闲
        /// </summary>
        public void TryIdle()
        {
            this._agentMonitor.TryIdle();
        }

        /// <summary>
        /// 尝试攻击
        /// </summary>
        public void TryAttack()
        {
            this._agentMonitor.TryAttack();
        }



    }
}