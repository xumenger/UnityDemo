using UnityEngine;

namespace xum.action
{
    /// <summary>
    /// Add in 2021-09-21
    /// 敌人控制模块
    /// 该脚本需要开发者显式挂载到敌人游戏物体上
    /// 
    /// </summary>
    public class EnemyAIController : MonoBehaviour
    {
        FSMManager fsmManager;
        InputSystem inputSystem;

        // 游戏物体的组件
        CharacterController controller;
        Animator animator;


        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            // 获取基础的组件
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();


            // TODO 使用键鼠输入 or 手柄输入，可配置化
            inputSystem = new EnemyAIInputSystem();

            // 动画有限状态机
            fsmManager = new EnemyAIFSMManager(gameObject, this, controller, animator, inputSystem);
        }


        /// <summary>
        /// Update is called once per frame
        /// 
        /// </summary>
        void Update()
        {
            if (inputSystem.IsJump())
                fsmManager.ChangeToState(StateEnum.eJumpUp);

            if (inputSystem.IsMove())
                fsmManager.ChangeToState(StateEnum.eMove);

            if (inputSystem.IsAttack())
                fsmManager.ChangeToState(StateEnum.eSlashRightHand);

            if (inputSystem.IsKick())
                fsmManager.ChangeToState(StateEnum.eKick);


            fsmManager.OnUpdate();

        }
    }
}