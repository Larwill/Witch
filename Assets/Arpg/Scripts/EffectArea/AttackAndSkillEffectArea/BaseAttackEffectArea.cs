using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arpg.EffectArea.AttackAndSkillEffectArea
{
    
    public class BaseAttackEffectArea:MonoBehaviour
    {
        [Tooltip("伤害判定的中心位置")]public Vector3 offset;
        [Tooltip("判定伤害的延迟时间")]public float delayTime;
        [Tooltip("判定伤害的持续时间")]public float durationTime;
        [Tooltip("击退的参考半径")]public float radius;
        [Tooltip("最大击退距离")]public float maxRapel; //最大击退距离
        [Tooltip("击中后产生的硬直时间")]public float hardStraightTime = 0.5f;
        [Tooltip("击中后产生的击退时间，应该小于硬直时间")]public float beAttackBackTime = 0.2f;
        public Collider trigger;
        private AgentMonitor self;
        private List<GameObject> targets;
        private Coroutine attackCaculator;
        private float speedRate = 1f;
        public void StartAttack(AgentMonitor self,float speedRate)
        {
            this.self = self;
            this.speedRate = speedRate;
            targets = new List<GameObject>();
            var hitDir = new Vector3(self._agentView.GetX(),0,0);
            var origin = self.transform.position + new Vector3(offset.x*hitDir.x,offset.y,offset.z)  ;
            transform.position = origin;
            gameObject.SetActive(true);
            if (attackCaculator != null)
            {
                StopCoroutine(attackCaculator);
            }
            attackCaculator = StartCoroutine(WaitToCaculate());
        }

        private IEnumerator WaitToCaculate()
        {
            trigger.enabled = false;
            yield return  new WaitForSeconds(delayTime*speedRate);
            trigger.enabled = true;
            yield return new WaitForSeconds(durationTime*speedRate);
            trigger.enabled = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            var g = other.gameObject;
            if (targets.Contains(g))
            {
                return;
            }
            targets.Add(g);
            var dir = g.transform.position - self.transform.position;
            float rate = 1f - Mathf.Clamp01(Mathf.Abs(dir.x)/radius);
            float rapelDst = rate * maxRapel;
            dir = dir.normalized * rapelDst;
            self.AttackTarget(g,dir,hardStraightTime,beAttackBackTime);       
        }

    }
}