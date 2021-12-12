using UnityEngine;

namespace xum.action
{

    /// <summary>
    /// Add in 2021-09-21
    ///
    /// 玩家控制模块
    /// 该脚本需要显式有开发者挂载到玩家游戏物体上
    /// 
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        FSMManager fsmManager;
        InputSystem inputSystem;

        // 角色控制器（还有一个点，角色控制器它也是一个碰撞体！）
        CharacterController controller;
        // 动画状态机
        Animator animator;

        Camera camera;


        /// <summary>
        /// Start is called before the first frame update
        /// 
        /// </summary>
        void Start()
        {
            // 获取基础的组件
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            camera = Camera.main;


            // TODO 使用键鼠输入 or 手柄输入，可配置化
            inputSystem = new PlayerKeyMouseInputSystem(camera);

            // 动画有限状态机
            fsmManager = new PlayerFSMManager(gameObject, this, controller, animator, inputSystem);
        }


        /// <summary>
        /// Update is called once per frame
        /// 
        /// </summary>
        void Update()
        {
            // 是否跳跃
            if (inputSystem.IsJump())
            {
                fsmManager.ChangeToState(StateEnum.eJumpUp);
            }

            // 是否移动
            if (inputSystem.IsMove())
            {
                fsmManager.ChangeToState(StateEnum.eMove);
            }

            // 是否攻击
            if (inputSystem.IsAttack())
            {
                fsmManager.ChangeToState(StateEnum.eSlashRightHand);
            }

            // 是否踢腿
            if (inputSystem.IsKick())
            {
                fsmManager.ChangeToState(StateEnum.eKick);
            }

            fsmManager.OnUpdate();

        }


        /// <summary>
        /// 
        /// </summary>
        private void FixedUpdate()
        {
            fsmManager.OnFixedUpdate();
        }


        /// <summary>
        /// 动画IK 相关处理
        /// 
        /// </summary>
        /// <param name="layerIndex"></param>
        private void OnAnimatorIK(int layerIndex)
        {
            fsmManager.OnAnimatorIK(layerIndex);
        }


        /// <summary>
        /// 需要为玩家增加CharacterController 组件
        /// 使用触发器的性能怎么样？
        ///
        /// 发布如下事件：
        /// 1. EventPlayerStartClimb
        /// 
        /// </summary>
        /// <param name="collider"></param>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Debug.LogWarning("hit: " + hit.collider.tag);

            // 不使用射线检测，而是替换为触发器触发攀爬事件
            if (hit.collider.tag == TagEnum.Tag_Wall)
            {
                // 这里使用Layer 判断是否走到Wall 前面，也可以使用Tag
                // 待优化点：
                // 1. 不使用Tag，而是使用collider 的高度等熟悉判断，这样就不需要手动设置Layer 或Tag
                // 2. 需要增加判断玩家与Wall 的夹角，否则动作切换会不好

                // 计算玩家面向的方向与墙的法线的反方向的夹角的余弦值
                float cosAngle = Vector3.Dot(transform.forward.normalized, -hit.normal.normalized);

                // 大于0.866，说明两者的夹角小于30，这个时候是玩家相对面向墙的角度，切换成攀爬比较合理
                // 不加上这个夹角判断，可能出现玩家与墙平行站位的时候，也切换到爬墙，不符合常识
                if (cosAngle > 0.866)
                {
                    // 创建事件对象，保存此事件发生时候的上下文信息
                    EventPlayerStartClimb eventPlayerStartClimb = new EventPlayerStartClimb(gameObject, hit.gameObject, hit.normal);

                    // 发布事件，回调攀爬事件对应的处理类
                    fsmManager.publishEvent(eventPlayerStartClimb);
                }
            }

            ///////////////////////////////////////////////////////
            ///
            // 其他事件检查

        }


        /// <summary>
        /// 切换到指定动作状态
        /// </summary>
        /// <param name="stateId"></param>
        public void ChangeToState(StateEnum stateId)
        {
            fsmManager.ChangeToState(stateId);
        }


        // 射线长度
        public float wallRayLength = 1;

        /// <summary>
        /// 检查玩家是否可以攀爬
        ///
        /// 2021-11-28 已经替换为使用触发器检查是否需要进入攀爬状态
        /// 该方法已弃用
        /// 
        /// </summary>
        /// 
        private bool checkPlayerCanClimb()
        {
            Vector3 origin = gameObject.transform.position;
            Vector3 dir = gameObject.transform.forward;

            // 使用射线检测是否走到墙边，hit 表示射线命中墙的位置
            RaycastHit hit;
            Debug.DrawRay(origin, dir * wallRayLength, Color.yellow);

            // ~LayerMask.NameToLayer("Wall") 实现只有与“Wall”层发生碰撞时才返回True
            // 这样就有一个条件，就是如果玩家想实现攀爬，那么攀爬的对象需要设置为Wall
            // 但是我更想实现的是判断，如果面前的对象比玩家高才爬墙，这样就不需要专门在Unity 中设置层来进行划分
            // 希望是完全在代码中实现这个逻辑，减少配置
            if (Physics.Raycast(origin, dir, out hit, wallRayLength, ~LayerMask.NameToLayer("Wall")))
            {
                Debug.Log("Wall");
                return true;
            }

            return false;
        }
    }

}