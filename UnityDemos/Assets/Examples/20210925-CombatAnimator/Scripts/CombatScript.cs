using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace example.y20210925
{
    public class CombatScript : MonoBehaviour
    {
        private EnemyManager enemyManager;        // 敌人管理器
        private EnemyDetection enemyDetection;    // 敌人探测
        private MovementInput movementInput;      // 移动输入控制
        private Animator animator;                // 动画控制器
        private CinemachineImpulseSource impulseSource;   // CinemachineImpulseSource 这个类的作用是什么？

        [Header("Target")]
        private EnemyScript lockedTarget;         // 被主角锁定的敌人

        [Header("Combat Settings")]
        [SerializeField] private float attackCooldown;

        // 这些状态如何用状态模式重构、优化？
        [Header("States")]
        public bool isAttackingEnemy = false;     // 是否正在攻击敌人
        public bool isCountering = false;         // 是否正在反击

        [Header("Public References")]
        [SerializeField] private Transform punchPosition;                // 拳击位置
        [SerializeField] private ParticleSystemScript punchParticle;     // ParticleSystemScript是什么类？
        [SerializeField] private GameObject lastHitCamera;
        [SerializeField] private Transform lastHitFocusObject;

        // Coroutines
        private Coroutine counterCoroutine;  // 反击协程
        private Coroutine attackCoroutine;   // 攻击协程
        private Coroutine damageCoroutine;   // 受伤协程

        [Space]

        // Events
        // 好好研究UnityEvent、事件模式
        public UnityEvent<EnemyScript> OnTrajectory;
        public UnityEvent<EnemyScript> OnHit;
        public UnityEvent<EnemyScript> OnCounterAttack;

        int animationCount = 0;
        string[] attacks;

        private void Start()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            animator = GetComponent<Animator>();
            enemyDetection = GetComponentInChildren<EnemyDetection>();
            movementInput = GetComponent<MovementInput>();
            impulseSource = GetComponentInChildren<CinemachineImpulseSource>();    // CinemachineImpulseSource 目前还没有弄明白
        }

        // This function gets called whenever the player inputs the punch action
        void AttackCheck()
        {
            // 如果正在攻击敌人，则直接退出
            if (isAttackingEnemy)
                return;

            // check to see if the detection behavior has an enemy set
            if (enemyDetection.CurrentTarget() == null)
            {
                if (enemyManager.AliveEnemyCount() == 0)
                {
                    Attack(null, 0);
                    return;
                }
                else
                {
                    lockedTarget = enemyManager.RandomEnemy();
                }
            }

            // if the player is moving the movement input, use the "directional" detection to determine the enemy
            if (enemyDetection.InputMagnitude() > 0.2f)
            {
                lockedTarget = enemyDetection.CurrentTarget();
            }

            // extra check to see if the locked target was set
            if (lockedTarget == null)
            {
                // 随机找一个作为被锁定的敌人
                lockedTarget = enemyManager.RandomEnemy();
            }

            // Attack Target
            Attack(lockedTarget, TargetDistance(lockedTarget));
        }

        public void Attack(EnemyScript target, float distance)
        {
            // types of attack animation
            attacks = new string[] { "AirKick", "AirKick2", "AirPunch", "AirKick3" };

            // attack nothing in case target is null
            if (target == null)
            {
                AttackType("GroundPunch", 0.2f, null, 0);
                return;
            }

            if (distance < 15)
            {
                animationCount = (int)Mathf.Repeat((float)animationCount + 1, (float)attacks.Length);
                string attackString = isLastHit() ? attacks[Random.Range(0, attacks.Length)] : attacks[animationCount];
                AttackType(attackString, attackCooldown, target, 0.65f);
            }
            else
            {
                lockedTarget = null;
                AttackType("GroundPunch", 0.2f, null, 0);
            }

            // change impulse
            impulseSource.m_ImpulseDefinition.m_AmplitudeGain = Mathf.Max(3, 1 * distance);
        }


        // 有的时候动作看起来没有力量，是因为动画播放的速度太慢了
        // 可以在Animator 中设置Speed 变大来调整播放速度，提升力量感和打击感
        void AttackType(string attackTrigger, float cooldown, EnemyScript target, float movementDuration)
        {
            animator.SetTrigger(attackTrigger);

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

            attackCoroutine = StartCoroutine(AttackCoroutine(isLastHit() ? 1.5f : cooldown));

            // check if last enemy
            if (isLastHit())
            {
                StartCoroutine(FinalBlowCoroutine());
            }

            if (target == null)
                return;

            // 敌人停止运动，玩家向着敌人的方向移动
            target.StopMoving();
            MoveTowardsTarget(target, movementDuration);

            IEnumerator AttackCoroutine(float duration)
            {
                movementInput.acceleration = 0;

                // 攻击前的设置
                isAttackingEnemy = true;         // 玩家正在向敌人进行攻击
                movementInput.enabled = false;   // 输入系统暂时置为不可用

                // 攻击持续时间
                yield return new WaitForSeconds(duration);

                // 攻击后恢复之前的设置
                isAttackingEnemy = false;
                yield return new WaitForSeconds(0.2f);
                movementInput.enabled = true;

                LerpCharacterAcceleration();
            }

            // 这个是在当攻击最后一个敌人的时候启动的协程
            // 用于实现时间减速，增加击打最后一个敌人的视觉效果
            IEnumerator FinalBlowCoroutine()
            {
                // 放慢0.5倍
                Time.timeScale = 0.5f;

                lastHitCamera.SetActive(true);
                lastHitFocusObject.position = lockedTarget.transform.position;
                yield return new WaitForSecondsRealtime(2);
                lastHitCamera.SetActive(false);

                // 恢复时间速度
                Time.timeScale = 1f;
            }
        }

        void MoveTowardsTarget(EnemyScript target, float duration)
        {
            // OnTrajectory？
            OnTrajectory.Invoke(target);

            // 使用DOTween 实现
            transform.DOLookAt(target.transform.position, 0.2f);
            transform.DOMove(TargetOffset(target.transform), duration);
        }


        // 反击检查
        void CounterCheck()
        {
            // initial check
            if (isCountering || isAttackingEnemy || !enemyManager.AnEnemyIsPreparingAttack())
            {
                return;
            }

            // 选择一个最近的敌人
            lockedTarget = ClosestCounterEnemy();
            OnCounterAttack.Invoke(lockedTarget);

            if (TargetDistance(lockedTarget) > 2)
            {
                Attack(lockedTarget, TargetDistance(lockedTarget));
                return;
            }

            float duration = 0.2f;
            animator.SetTrigger("Dodge");
            transform.DOLookAt(lockedTarget.transform.position, 0.2f);
            transform.DOMove(transform.position + lockedTarget.transform.forward, duration);

            if (counterCoroutine != null)
            {
                StopCoroutine(counterCoroutine);
            }
            counterCoroutine = StartCoroutine(CounterCoroutine(duration));

            IEnumerator CounterCoroutine(float duration)
            {
                isCountering = true;   // 正在反击
                movementInput.enabled = false;
                yield return new WaitForSeconds(duration);
                Attack(lockedTarget, TargetDistance(lockedTarget));

                isCountering = false;  // 反击结束
            }
        }

        float TargetDistance(EnemyScript target)
        {
            return Vector3.Distance(transform.position, target.transform.position);
        }

        public Vector3 TargetOffset(Transform target)
        {
            Vector3 position;
            position = target.position;
            return Vector3.MoveTowards(position, transform.position, 0.95f);
        }

        public void HitEvent()
        {
            // 当没有目标敌人，或者敌人的数量为0，直接退出
            if (lockedTarget == null || enemyManager.AliveEnemyCount() == 0)
            {
                return;
            }

            OnHit.Invoke(lockedTarget);

            // Polish。在指定位置播放攻击特效
            // 注意取消勾选ParticleSystem 上的Looping 参数，否则这个特效会一直循环播放
            // 而我们希望的是在发起攻击的时候进行播放！
            punchParticle.PlayParticleAtPosition(punchPosition.position);
        }

        public void DamageEvent()
        {
            animator.SetTrigger("Hit");

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
            damageCoroutine = StartCoroutine(DamageCoroutine());

            IEnumerator DamageCoroutine()
            {
                movementInput.enabled = false;
                yield return new WaitForSeconds(0.5f);
                movementInput.enabled = true;
                LerpCharacterAcceleration();
            }
        }

        EnemyScript ClosestCounterEnemy()
        {
            float minDistance = 100;
            int finalIndex = 0;

            for (int i = 0; i < enemyManager.allEnemies.Length; i++)
            {
                EnemyScript enemy = enemyManager.allEnemies[i].enemyScript;

                if (enemy.IsPreparingAttack())
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < minDistance)
                    {
                        minDistance = Vector3.Distance(transform.position, enemy.transform.position);
                        finalIndex = i;
                    }
                }
            }

            return enemyManager.allEnemies[finalIndex].enemyScript;
        }

        void LerpCharacterAcceleration()
        {
            movementInput.acceleration = 0;
            DOVirtual.Float(0, 1, 0.6f, ((acceleration) => movementInput.acceleration = acceleration));
        }

        bool isLastHit()
        {
            if (lockedTarget == null)
            {
                return false;
            }

            return enemyManager.AliveEnemyCount() == 1 && lockedTarget.health <= 1;
        }

        #region Input
        private void OnCounter()
        {
            CounterCheck();
        }

        private void OnAttack()
        {
            AttackCheck();
        }

        #endregion
    }
}