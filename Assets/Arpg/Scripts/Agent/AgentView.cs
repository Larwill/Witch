using System;
using System.Collections.Generic;
using AnyPortrait;
using UnityEngine;

namespace Arpg
{
    public static class AgentViewID
    {
        public static int idle = 0;
        public static int run = 1;
        public static int attack = 2;
        public static int die = 3;
        public static int beAttack = 4;
    }
    
    public class AgentView:MonoBehaviour
    {
        private int currentID = AgentViewID.idle;
        private apPortrait _apPortrait;
        public string idleName = "Idle";
        public string runName = "Run";
        public List<string> attackNames = new List<string>(){"Attack"} ;
        public string beAttackName = "Run";
        public string jumpName = "jump";
        public string dieName = "Run";
        private float attackSpeedRate = 1f;
        private float moveSpeedRate = 1f;
        
        private void Awake()
        {
            currentID = AgentViewID.idle;
            _apPortrait = this.GetComponentInChildren<apPortrait>();
            
        }

        public float GetX()
        {
            return -_apPortrait.transform.localScale.x;
        }
        
        public void TryIdleView()
        {
            if (currentID != AgentViewID.idle)
            {
                currentID = AgentViewID.idle;
                _apPortrait.CrossFade(idleName,0.1f);
            }
        }
        
        public void TryRunView(Vector3 direction)
        {
            if (currentID != AgentViewID.run)
            {
                currentID = AgentViewID.run;
                _apPortrait.CrossFade(runName, 0.1f);
            }
            this.ResetViewDirection(direction);
        }

        private void ResetViewDirection(Vector3 direction)
        {
            if (direction.x == 0)
            {
                return;
            }
            if (direction.x < 0)
            {
                _apPortrait.transform.localScale = new Vector3(1,1,1);    
            }
            else
            {
                _apPortrait.transform.localScale = new Vector3(-1,1,1);
            }
        }

        private void Start()
        {
            currentID = AgentViewID.idle;
            _apPortrait.CrossFade(idleName,0.1f);
        }

        public void TryAttackView(int currentAttackId)
        {
             currentID = AgentViewID.attack;
             _apPortrait.CrossFade(attackNames[currentAttackId], 0.1f);
        }

        /// <summary>
        /// 动画修改攻击速度
        /// </summary>
        /// <param name="addOnRate"></param>
        public void ChangeAttackSpeedRate(float addOnRate)
        {
            this.attackSpeedRate += addOnRate;
            var rate = this.attackSpeedRate > 0.2f ? this.attackSpeedRate : 0.2f;
            foreach (string attackName in attackNames)
            {
                _apPortrait.SetAnimationSpeed(attackName,rate);    
            }
        }

        /// <summary>
        /// 动画修改奔跑速度
        /// </summary>
        /// <param name="addOnRate"></param>
        public void ChangeRunSpeedRate(float addOnRate)
        {
            this.moveSpeedRate += addOnRate;
            var rate = this.moveSpeedRate > 0.2f ? this.moveSpeedRate : 0.2f;
            _apPortrait.SetAnimationSpeed(runName,rate);
        }
        

        public void TryDieView()
        {
            if (currentID != AgentViewID.die)
            {
                currentID = AgentViewID.die;
                _apPortrait.CrossFade(dieName,0.1f);    
            }
        }

        public void TryBeAttackView()
        {
            currentID = AgentViewID.beAttack;
            _apPortrait.CrossFade(beAttackName, 0.1f);
        }

        public void TryJump()
        {
            _apPortrait.CrossFade(jumpName,0.1f);
        }
    }
    
    
    
}