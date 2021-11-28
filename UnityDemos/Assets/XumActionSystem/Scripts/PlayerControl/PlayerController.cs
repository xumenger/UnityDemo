using UnityEngine;

namespace xum.action
{

    /**
     * Add in 2021-09-21
     * 玩家控制模块
     * 
     * 该脚本需要显式有开发者挂载到玩家游戏物体上
     */
    public class PlayerController : MonoBehaviour
    {
        FSMManager fsmManager;
        InputSystem inputSystem;

        // 游戏物体的组件
        CharacterController controller;
        Animator animator;

        Camera camera;


        // Start is called before the first frame update
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


        // Update is called once per frame
        void Update()
        {
            // 是否跳跃
            if (inputSystem.IsJump())
                fsmManager.ChangeToState(StateEnum.eJumpUp);

            // 是否移动
            if (inputSystem.IsMove())
                fsmManager.ChangeToState(StateEnum.eMove);

            // 是否攻击
            if (inputSystem.IsAttack())
                fsmManager.ChangeToState(StateEnum.eSlashRightHand);

            // 是否踢腿
            if (inputSystem.IsKick())
                fsmManager.ChangeToState(StateEnum.eKick);

            // 检查玩家是否可以攀爬
            if (checkPlayerCanClimb())
            {
                // 创建事件对象，保存此事件发生时候的上下文信息
                EventPlayerStartClimb eventPlayerStartClimb = new EventPlayerStartClimb(gameObject);
                // 发布事件，回调攀爬事件对应的处理类
                fsmManager.publicEvent(eventPlayerStartClimb);
            }
                

            fsmManager.OnUpdate();

        }

        private void FixedUpdate()
        {
            fsmManager.OnFixedUpdate();
        }


        private void OnAnimatorIK(int layerIndex)
        {
            fsmManager.OnAnimatorIK(layerIndex);
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
        /// </summary>
        private bool checkPlayerCanClimb()
        {
            Vector3 origin = gameObject.transform.position;
            Vector3 dir = gameObject.transform.forward;

            // 使用射线检测是否走到墙边，hit 表示射线命中墙的位置
            RaycastHit hit;
            Debug.DrawRay(origin, dir * wallRayLength, Color.yellow);
            if (Physics.Raycast(origin, dir, out hit, wallRayLength))
            {
                return true;
            }

            return false;
        }
    }

}