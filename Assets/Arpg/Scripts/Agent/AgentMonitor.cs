using System;
using System.Collections;
using System.Collections.Generic;
using Arpg.EffectArea;
using Arpg.EffectArea.AttackAndSkillEffectArea;
using Arpg.Scripts.Buff;
using Arpg.Scripts.Type;
using Arpg.Scripts.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Arpg
{
    [RequireComponent(typeof(AgentView))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class AgentMonitor : MonoBehaviour
    {
        public float moveSpeed = 7f;
        public float runSpeed = 20f;
        public List<float> attackTimes; //每一段的持续时间
        public List<int> baseAttackValues; //每一段的攻击伤害
        private float multiplyAttackRate = 1f;
        private float multiplyAttackSpeedRate = 1f;
        private float antiAttackRate = 0f;

        public bool igonreHardStraight = false; //忽略硬直
        
        [HideInInspector]public AgentView _agentView;
        private bool isStrikeFly = false; //正在被击飞
        private bool isVertigo = false; //正在被眩晕
        private bool isImprisonment = false; //正在被禁锢
        private bool isHardStraight = false; //正在被攻击,产生了硬直
        private bool isAttacking = false; //正在攻击
        private bool isWaitingNextComb = false; //正在等待下一个comb

        private float moveRate = 1f; //移动奔跑系数
        private int attackCount; //攻击comb上限
        private int currentAttackID = 0;
        private float currentMaxAttackTime = 0f;
        private float currentAttackTime = 0;

        private float currentMaxHardStraightTime = 0.3f;
        private float currentHardStraightTime = 0f; //当前硬直时间
        private float currentWaitingNextCombTime = 0f; //当前等待下一个comb的时间
        private float currentImprisonmentTime; //当前的击飞时间
        private float currentMaxImprisonmentTime = 0.2f; //当前的最大击飞时间
        private float currentMaxBeAttackBackTime = 0.2f; //当前最大被击退时间
        
        private const float currentMaxWaitingNextCombTime = 0.5f;
        public int health = 100; //生命值
        public int maxHealth = 100; //最大生命值
        private float velocityY = -9.8f;
        public float addOnVelocity = 10;
        private Rigidbody _rigidbody;

        public List<int> enemyLayers;
        public BaseAttackEffectArea attackEffectArea; //攻击效果区域

        private InformationUI informationUi;

        public event Action<Vector3> onPositionChange;
        public event Action<int> onMaxHealthChange;
        public event Action<int> onHealthChange;
        public event System.Action onDeath;

        private List<IAddBuff> _addBuffs;
        private List<IAttackBuff> _attackBuffs;
        private List<IMultiplyBuff> _multiplyBuffs;
        private List<IBeAttackBuff> _beAttackBuffs;
        private List<IReleaseSkillBuff> _releaseSkillBuffs;
        
        private NavMeshAgent navmeshagent;
        private CapsuleCollider capcollider;
        private bool isPlayerControl = false;

        private void Awake()
        {
            navmeshagent = GetComponent<NavMeshAgent>();
            capcollider = GetComponent<CapsuleCollider>();
            this._rigidbody = GetComponent<Rigidbody>();

            _agentView = this.GetComponent<AgentView>();
            //相关属性初始化
            attackCount = this.attackTimes.Count;
            currentAttackID = 0;
            this.multiplyAttackRate = 1f;
            this.antiAttackRate = 0f;
            this.multiplyAttackSpeedRate = 1f;

            this._addBuffs = new List<IAddBuff>();
            this._attackBuffs = new List<IAttackBuff>();
            this._multiplyBuffs = new List<IMultiplyBuff>();
            this._beAttackBuffs = new List<IBeAttackBuff>();
            this._releaseSkillBuffs = new List<IReleaseSkillBuff>();
        }

        public void AddAddBuff(IAddBuff addBuff)
        {
            this._addBuffs.Add(addBuff);
            addBuff.DoBuff(() => { });
        }

        public void AddAttackBuff(IAttackBuff attackBuff)
        {
            this._attackBuffs.Add(attackBuff);
        }

        public void AddMultiplyBuff(IMultiplyBuff multiplyBuff)
        {
            this._multiplyBuffs.Add(multiplyBuff);
            multiplyBuff.DoBuff(() => { });
        }

        public void AddBeAttackBuff(IBeAttackBuff beAttackBuff)
        {
            this._beAttackBuffs.Add(beAttackBuff);
            beAttackBuff.DoBuff(() => { });
        }

        public void AddReleaseSkillBuff(IReleaseSkillBuff releaseSkillBuff)
        {
            this._releaseSkillBuffs.Add(releaseSkillBuff);
        }


        private void Start()
        {
            this.informationUi = InformationUI.NewInstance(this);
        }

        public bool CanRun
        {
            get
            {
                return (isStrikeFly == false) && (isVertigo == false) && (isImprisonment == false) &&
                       (isAttacking == false) && (Alive == true) && (isHardStraight == false);
            }
        }

        public bool CanIdle
        {
            get
            {
                return (isStrikeFly == false) && (isVertigo == false) && (isImprisonment == false) &&
                       (isAttacking == false);
            }
        }

        public bool Alive
        {
            get { return this.health > 0; }
        }

        public bool CanAttackComb
        {
            get
            {
                return (isStrikeFly == false) && (isVertigo == false) && (isImprisonment == false) &&
                       (isAttacking == false) && (isHardStraight == false) &&
                       (isWaitingNextComb == true || currentAttackID == 0);
            }
        }

        private AgentMonitor targetEnemy;
        private AgentMonitor attackOnTarget;

        public AgentMonitor AttackOnTarget
        {
            get
            {
                return  attackOnTarget;
            }
        }
       


        public AgentMonitor TargetEnemy
        {
            get { return targetEnemy; }
            set { targetEnemy = value; }
        }

        private bool isJumping = false;
        public bool CanJump
        {
            get { return true; }
        }


        public void AddOnMoveRate(float v)
        {
            moveRate += v;
        }


        public bool TryRun(Vector3 direction)
        {
            if (CanRun == true)
            {
                navmeshagent.isStopped = false;
                navmeshagent.speed = runSpeed * moveRate;
                navmeshagent.acceleration = runSpeed * moveRate*2;

              //  var targetPosition = (direction * runSpeed * moveRate)*Time.deltaTime;
               var targetPosition = (direction * runSpeed * moveRate)*Time.deltaTime;
              
//                Ray ray  = new Ray(transform.position+ new Vector3(targetPosition.x,100,targetPosition.z),Vector3.down);
//                RaycastHit raycastHit;
//                if (Physics.Raycast(ray, out raycastHit,1000,31))
//                {
//                    targetPosition = raycastHit.point;
//                }
//                else
//                {
//                    return false;
//                }
////                

                navmeshagent.Move(targetPosition);;
            // navmeshagent.destination = targetPosition;
                navmeshagent.updatePosition = true;
                _agentView.TryRunView(direction);
                if (this.onPositionChange != null)
                {
                    this.onPositionChange(transform.position);
                }

                return true;
            }

            return false;
        }

        public bool TryIdle()
        {
            if (CanIdle == true)
            {
                _agentView.TryIdleView();
                return true;
            }

            return false;
        }


        public bool TryAttack()
        {
            if (CanAttackComb == true)
            {
                this.attackOnTarget = null;
                var attackRate = this.multiplyAttackSpeedRate > 0.2f ? this.multiplyAttackSpeedRate : 0.2f;
                
                currentMaxAttackTime = attackTimes[currentAttackID]*attackRate;
                currentAttackTime = 0;
                isAttacking = true;
                isWaitingNextComb = false;
                
                if (attackEffectArea.gameObject.activeSelf == true)
                {
                    attackEffectArea.gameObject.SetActive(false);
                }
                attackEffectArea.StartAttack(this,attackRate);
                
                _agentView.TryAttackView(currentAttackID);
                return true;
            }
            else
            {
                return false;
            }
        }

//        public void TryJump()
//        {
//            if (CanJump)
//            {
//                isJumping = true;
//                _rigidbody.AddForce(new Vector3(0,addOnVelocity,0));
//                _agentView.TryJump();
//            }
//        }
        

        public void TryBeAttack(AgentMonitor attacker, int reduceHealth, int attackType)
        {
            if (this.Alive == false)
            {
                return;
            }

            this.navmeshagent.enabled = false;
            this.navmeshagent.enabled = true;
            this.ReduceHeath(attacker, reduceHealth, attackType);
            if (this.health <= 0) //如果生命值小于0
            {
                this.RemoveAllController();
                this.attackEffectArea.gameObject.SetActive(false);
                this._agentView.TryDieView();
                if (onDeath != null)
                {
                    onDeath();
                }

                Destroy(gameObject, 2);
                return;
            }

            //如果是反伤，就不表现被攻击的硬直 ,todo 如果需要做反伤硬直
            if (attackType == AttackType.antiAttack)
            {
                return;
            }

            //下面是被攻击的表现
            if (igonreHardStraight == true)
            {
            }
            else
            {
                this.isAttacking = false;
                isWaitingNextComb = true;

                if (this.attackEffectArea.gameObject.activeSelf == true)
                {
                    this.attackEffectArea.gameObject.SetActive(false);
                }

                this._agentView.TryBeAttackView();
                isHardStraight = true;
                this.currentHardStraightTime = 0f;
            }
        }

        private void RemoveAllController()
        {
            navmeshagent.enabled = false;
            capcollider.enabled = false;
        }

        //扣血，在这里实现复杂的扣血机制
        private void ReduceHeath(AgentMonitor attacker, int reduceHealth, int attackType)
        {
            var realReduceValue = reduceHealth;

            if (attackType != AttackType.antiAttack)
            {
                if (this.antiAttackRate > 0)
                {
                    var antiValue = Convert.ToInt32(this.antiAttackRate * realReduceValue);
                    attacker.TryBeAttack(this, antiValue, AttackType.antiAttack);
                }
            }

            this.health -= reduceHealth;
            if (this.onHealthChange != null)
            {
                this.onHealthChange(this.health);
            }
        }

        public System.Action updateAction;
        private Vector3 attackBackDir;

        private void Update()
        {

            
            if (Alive == false)
            {
                _agentView.TryDieView();
                return;
            }
            
            //是否正在攻击
            if (isAttacking == true)
            {
                currentAttackTime += Time.deltaTime;
                if (currentAttackTime >= currentMaxAttackTime)
                {
                    isAttacking = false;
                    currentAttackID += 1;

                    if (currentAttackID >= attackCount)
                    {
                        currentAttackID = 0;
                    }
                    else
                    {
                        isWaitingNextComb = true;
                        this.currentWaitingNextCombTime = 0f;
                    }
                }
            }

            //是否正在等待下一次comb
            if (isWaitingNextComb == true)
            {
                currentWaitingNextCombTime += Time.deltaTime;
                if (currentWaitingNextCombTime >= currentMaxWaitingNextCombTime)
                {
                    isWaitingNextComb = false;
                    this.currentAttackID = 0; //超过了等待时间就重新开始comb
                }
            }

            //是否正在硬直
            if (isHardStraight == true)
            {
                this.currentHardStraightTime += Time.deltaTime;
                this.navmeshagent.isStopped = false;
                
                //是否在被击退中
                if (currentHardStraightTime < this.currentMaxBeAttackBackTime)
                {
                    this.navmeshagent.Move(this.attackBackDir*Time.deltaTime);    
                }
                
                //是否在硬直中
                if (currentHardStraightTime > this.currentMaxHardStraightTime)
                {
                    this.isHardStraight = false;
                    this._agentView.TryIdleView();
                }
            }

            if (updateAction != null)
            {
                updateAction();
            }
            
        }

        private void LateUpdate()
        {
       //     navmeshagent.velocity = Vector3.zero;
        }

        public void AttackTarget(GameObject target,Vector3 backDir,float hardStraightTime,float beAttackBackTime)
        {
            var agentMonitor = target.GetComponent<AgentMonitor>();
            var layer = target.layer;
            if (enemyLayers.Contains(layer))
            {
                if (agentMonitor != null)
                {
                    this.attackOnTarget = agentMonitor;
                    Vector3 dir = new Vector3(backDir.x, 0, 0);
                    agentMonitor.attackBackDir = dir;
                    agentMonitor.currentMaxHardStraightTime = hardStraightTime;
                    agentMonitor.currentMaxBeAttackBackTime = beAttackBackTime;
                    agentMonitor.TryBeAttack(this, this.GetCurrentAttackValue(), AttackType.normalAttack);
                }
            }
        }

        /// <summary>
        /// 当前实际攻击力
        /// </summary>
        /// <returns></returns>
        private int GetCurrentAttackValue()
        {
            int baseAttackValue = this.baseAttackValues[this.currentAttackID];
            return Convert.ToInt32(baseAttackValue * multiplyAttackRate);
        }


        public int GetMaxHealth()
        {
            return this.maxHealth;
        }

        public int GetCurrentHealth()
        {
            return this.health;
        }


        /// <summary>
        /// 修改最大生命值
        /// </summary>
        /// <param name="v"></param>
        public void AddOnMaxHealth(int v)
        {
            this.maxHealth += v;
            this.health += v;
            if (this.health <= 0)
            {
                this.health = 1;
            }

            if (this.onMaxHealthChange != null)
            {
                onMaxHealthChange(this.maxHealth);
            }

            if (this.onHealthChange != null)
            {
                this.onHealthChange(this.health);
            }
        }

        /// <summary>
        /// 设置为AI控制,todo 还有控制器的切换
        /// </summary>
        public void SetIsAIControl()
        {
            isPlayerControl = false;
        }

        /// <summary>
        /// 设置为玩家控制,todo 还有控制器的切换
        /// </summary>
        public void SetIsPlayerControl()
        {
            isPlayerControl = true;
        }

        /// <summary>
        /// 修改攻击比率
        /// </summary>
        /// <param name="multiplyRate"></param>
        public void ChangeMultiplyAttackRate(float multiplyRate)
        {
            this.multiplyAttackRate += multiplyRate;
        }

        /// <summary>
        /// 修改反伤比率
        /// </summary>
        /// <param name="addOnAntiAttackRate"></param>
        public void ChangeAntiAttackRate(float addOnAntiAttackRate)
        {
            this.antiAttackRate += addOnAntiAttackRate;
        }

        /// <summary>
        /// 直接永久增加攻击力
        /// </summary>
        /// <param name="attackValue"></param>
        public void AddOnAttackValue(int attackValue)
        {
            for (int i = 0; i < this.baseAttackValues.Count; i++)
            {
                this.baseAttackValues[i] += attackValue;
            }
        }

        /// <summary>
        /// 直接永久增加移动速度
        /// </summary>
        /// <param name="addOnSpeed"></param>
        public void AddMoveAndRunSpeed(float addOnSpeed)
        {
            this.moveSpeed += addOnSpeed;
            this.runSpeed += addOnSpeed;
        }


        public void TryNav(Vector3 targetPosition)
        {
            if (CanRun == true)
            {
                Vector3 direction =
                    targetPosition - transform.position;
                navmeshagent.speed = runSpeed;
                navmeshagent.acceleration = runSpeed * 3;
                navmeshagent.destination = targetPosition;
                
                _agentView.TryRunView(direction);
                if (this.onPositionChange != null)
                {
                    this.onPositionChange(transform.position);
                }
            }

        }

        public void TryAvoid(Vector3 position,float dst)
        {
            if (CanRun == true)
            {
                int xSign = Random.Range(0, 2);
                if (xSign == 0)
                {
                    xSign = -1;
                }
                Vector3 ChebyPosition = position + new Vector3(xSign*dst*10,0,0);
                navmeshagent.speed = runSpeed;
                navmeshagent.acceleration = runSpeed * 3;
                navmeshagent.destination = ChebyPosition;
                _agentView.TryRunView(ChebyPosition-transform.position);
                if (this.onPositionChange != null)
                {
                    this.onPositionChange(transform.position);
                }
            }
          
            
        }

        /// <summary>
        /// 攻速增加比率
        /// </summary>
        /// <param name="rate"></param>
        public void AddOnAttackSpeedRate(float rate)
        {
            this.multiplyAttackSpeedRate -= rate;
            Debug.Log(this.multiplyAttackSpeedRate);
        }
    }
}