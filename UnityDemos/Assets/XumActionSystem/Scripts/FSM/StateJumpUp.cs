using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class StateJumpUp : FSMState
    {
        CharacterController controller;

        // TODO 以下变量实现配置化
        public float jumpHeight = 4.0F;        // 跳跃高度
        public float gravity = 10.0F;          // 重力值，下降的速度
        public float durationTime = 0.8f;      // 该动作只应该播放一次，持续时间

        private Vector3 moveDirection = Vector3.zero;


        public StateJumpUp(GameObject gameObject,
                           Animator animator,
                           CharacterController controller,
                           FSMManager fsmManager) : base(EChangeType.eTrigger, gameObject, animator, fsmManager)
        {
            this.controller = controller;
        }


        public override void OnStart()
        {
            // 播放“JumpAttack”动画
            animator.SetTrigger(AnimatorEnum.Anim_T_JumpUp);

            // 设置跳跃的高度
            moveDirection.y = jumpHeight;

            // 设置该动作的持续时间后切换到上一个状态
            fsmManager.StartCoroutine(WaitForAWhile(durationTime));
        }


        public override void OnUpdate()
        {
            // 角色控制器实现角色移动
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }


        public override void OnEnd()
        {

        }


        public override void DoEvent(object param)
        {

        }
    }
}