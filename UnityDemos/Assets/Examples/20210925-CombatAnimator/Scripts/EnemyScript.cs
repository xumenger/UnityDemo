using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace example.y20210925
{
    public class EnemyScript : MonoBehaviour
    {
        // Declarations
        private Animator animator;
        private CombatScript playerCombat;       // 玩家攻击脚本
        private EnemyManager enemyManager;       // 敌人管理器
        private EnemyDetection enemyDetection;   // 敌人探测器
        private CharacterController characterController;

        [Header("Stats")]
        public int health = 3;
        private float moveSpeed = 1;
        private Vector3 moveDirection;

        [Header("States")]
        [SerializeField] private bool isPreparingAttack;
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isRetreating;          // 撤退
        [SerializeField] private bool isLockedTarget;
        [SerializeField] private bool isStunned;             // 打昏
        [SerializeField] private bool isWaiting = true;

        [Header("Polish")]
        [SerializeField] private ParticleSystem counterParticle;    // 反击粒子特效

        private Coroutine PrepareAttackCoroutine;
        private Coroutine RetreatCoroutine;
        private Coroutine DamageCoroutine;
        private Coroutine MovementCoroutine;

        // Events
        public UnityEvent<EnemyScript> OnDamage;
        public UnityEvent<EnemyScript> OnStopMoving;
        public UnityEvent<EnemyScript> OnRetreat;

        private void Start()
        {
            enemyManager = GetComponentInParent<EnemyManager>();

            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            playerCombat = FindObjectOfType<CombatScript>();
            enemyDetection = playerCombat.GetComponentInChildren<EnemyDetection>();

            playerCombat.OnHit.AddListener((x) => OnPlayerHit(x));                 // 添加监听器：玩家受到攻击时的回调
            playerCombat.OnCounterAttack.AddListener((x) => OnPlayerCounter(x));   // 
            playerCombat.OnTrajectory.AddListener((x) => OnPlayerTrajectory(x));   // 

            MovementCoroutine = StartCoroutine(EnemyMovement());
        }

        IEnumerator EnemyMovement()
        {
            // Waits until the enemy is not assigned to no action like attacking or retreating
            yield return new WaitUntil(() => isWaiting == true);

            int randomChance = Random.Range(0, 2);

            if (randomChance == 1)
            {
                int randomDir = Random.Range(0, 2);
                moveDirection = randomDir == 1 ? Vector3.right : Vector3.left;
                isMoving = true;
            }
            else
            {
                StopMoving();
            }

            yield return new WaitForSeconds(1);

            MovementCoroutine = StartCoroutine(EnemyMovement());
        }

        private void Update()
        {
            // Constanly look at player
            transform.LookAt(new Vector3(playerCombat.transform.position.x, transform.position.y, playerCombat.transform.position.z));

            // Only moves if the direction is set
            MoveEnemy(moveDirection);
        }


        // Listened event from Player Animation
        void OnPlayerHit(EnemyScript target)
        {
            if (target == this)
            {
                StopEnemyCoroutines();
                DamageCoroutine = StartCoroutine(HitCoroutine());

                enemyDetection.SetCurrentTarget(null);
                isLockedTarget = false;
                OnDamage.Invoke(this);

                health--;

                if (health <= 0)
                {
                    Death();
                    return;
                }

                animator.SetTrigger("Hit");
                transform.DOMove(transform.position - (transform.forward / 2), 0.3f).SetDelay(0.1f);

                StopMoving();
            }

            IEnumerator HitCoroutine()
            {
                isStunned = true;
                yield return new WaitForSeconds(0.5f);
                isStunned = false;
            }
        }

        void OnPlayerCounter(EnemyScript target)
        {
            if (target == this)
            {
                PrepareAttack(false);
            }
        }

        void OnPlayerTrajectory(EnemyScript target)
        {
            if (target == this)
            {
                StopEnemyCoroutines();
                isLockedTarget = true;
                PrepareAttack(false);
                StopMoving();
            }
        }

        void Death()
        {
            StopEnemyCoroutines();

            this.enabled = false;
            characterController.enabled = false;
            animator.SetTrigger("Death");
            enemyManager.SetEnemyAvailiablity(this, false);
        }

        public void SetRetreat()
        {
            StopEnemyCoroutines();

            RetreatCoroutine = StartCoroutine(PrepRetreat());

            IEnumerator PrepRetreat()
            {
                yield return new WaitForSeconds(1.4f);
                OnRetreat.Invoke(this);
                isRetreating = true;
                moveDirection = -Vector3.forward;
                isMoving = true;
                yield return new WaitUntil(() => Vector3.Distance(transform.position, playerCombat.transform.position) > 4);
                isRetreating = false;
                StopMoving();

                // Free
                isWaiting = true;
                MovementCoroutine = StartCoroutine(EnemyMovement());
            }
        }

        public void SetAttack()
        {
            isWaiting = false;

            PrepareAttackCoroutine = StartCoroutine(PrepAttack());

            IEnumerator PrepAttack()
            {
                PrepareAttack(true);
                yield return new WaitForSeconds(0.2f);
                moveDirection = Vector3.forward;
                isMoving = true;
            }
        }

        void PrepareAttack(bool active)
        {
            isPreparingAttack = active;

            if (active)
            {
                // 现在的粒子效果并不好，暂时关闭
                //counterParticle.Play();
            }
            else
            {
                StopMoving();
                counterParticle.Clear();
                counterParticle.Stop();
            }
        }

        void MoveEnemy(Vector3 direction)
        {
            // Set movespeed based on direction
            moveSpeed = 1;

            if (direction == Vector3.forward)
                moveSpeed = 5;
            if (direction == -Vector3.forward)
                moveSpeed = 2;

            // Set Animtor values
            animator.SetFloat("InputMagnitude", (characterController.velocity.normalized.magnitude * direction.z) / (5 / moveSpeed), 0.2f, Time.deltaTime);
            animator.SetBool("Strafe", (direction == Vector3.right || direction == Vector3.left));
            animator.SetFloat("StrafeDirection", direction.normalized.x, 0.2f, Time.deltaTime);

            // Don't do anything if isMoving is false
            if (!isMoving)
                return;

            Vector3 dir = (playerCombat.transform.position - transform.position).normalized;
            Vector3 pDir = Quaternion.AngleAxis(90, Vector3.up) * dir; //Vector perpendicular to direction
            Vector3 movedir = Vector3.zero;

            Vector3 finalDirection = Vector3.zero;

            if (direction == Vector3.forward)
                finalDirection = dir;
            if (direction == Vector3.right || direction == Vector3.left)
                finalDirection = (pDir * direction.normalized.x);
            if (direction == -Vector3.forward)
                finalDirection = -transform.forward;

            if (direction == Vector3.right || direction == Vector3.left)
                moveSpeed /= 1.5f;

            movedir += finalDirection * moveSpeed * Time.deltaTime;

            characterController.Move(movedir);

            if (!isPreparingAttack)
                return;

            // 判断敌人与玩家的距离是否小于2
            if (Vector3.Distance(transform.position, playerCombat.transform.position) < 2)
            {
                // 如果小于2，则停止移动
                StopMoving();

                if (!playerCombat.isCountering && !playerCombat.isAttackingEnemy)
                    // 如果玩家不处于防守、攻击状态，则敌人发起攻击
                    Attack();
                else
                    // 否则敌人准备攻击
                    PrepareAttack(false);
            }
        }

        private void Attack()
        {
            transform.DOMove(transform.position + (transform.forward / 1), 0.5f);
            animator.SetTrigger("AirPunch");
        }

        // 在动画上设置的动画事件，用于判断是否Hit 是否发生
        public void HitEvent()
        {
            // 如果敌人攻击，玩家不处于防守/攻击状态，则处理受伤逻辑
            if (!playerCombat.isCountering && !playerCombat.isAttackingEnemy)
                playerCombat.DamageEvent();

            PrepareAttack(false);
        }

        public void StopMoving()
        {
            isMoving = false;
            moveDirection = Vector3.zero;
            if (characterController.enabled)
                characterController.Move(moveDirection);
        }


        void StopEnemyCoroutines()
        {
            PrepareAttack(false);

            if (isRetreating)
            {
                if (RetreatCoroutine != null)
                {
                    StopCoroutine(RetreatCoroutine);
                }
            }

            if (PrepareAttackCoroutine != null)
                StopCoroutine(PrepareAttackCoroutine);

            if (DamageCoroutine != null)
                StopCoroutine(DamageCoroutine);

            if (MovementCoroutine != null)
                StopCoroutine(MovementCoroutine);
        }


        #region Public Booleas
        public bool IsAttackable()
        {
            return health > 0;
        }

        public bool IsPreparingAttack()
        {
            return isPreparingAttack;
        }

        public bool IsRetreating()
        {
            return isRetreating;
        }

        public bool IsLockedTarget()
        {
            return isLockedTarget;
        }

        public bool IsStunned()
        {
            return isStunned;
        }

        #endregion

    }
}