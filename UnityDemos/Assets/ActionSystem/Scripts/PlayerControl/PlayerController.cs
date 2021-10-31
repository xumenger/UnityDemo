using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{

    /**
     * Add in 2021-09-21
     * 玩家控制模块
     * 
     */
    public class PlayerController : MonoBehaviour
    {
        FSMManager fsmManager;
        InputSystem inputSystem;

        // 游戏物体的组件
        CharacterController controller;
        Animator animator;


        // Start is called before the first frame update
        void Start()
        {
            // 获取基础的组件
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();


            // TODO 使用键鼠输入 or 手柄输入，可配置化
            inputSystem = new PlayerKeyMouseInputSystem();

            // 动画有限状态机
            fsmManager = new PlayerFSMManager(transform, this, controller, animator, inputSystem);
        }


        // Update is called once per frame
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