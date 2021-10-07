using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    /**
     * 使用Blend Tree 将Idle、Walk、Run 等动作进行封装
     */
    public class StateMove : FSMState
    {
        InputSystem inputSystem;

        CharacterController controller;

        public float speed;
        private Vector3 moveDirection = Vector3.zero;

        float forward = 0.0F;
        float right = 0.0F;


        public StateMove(Transform transform,
                         Animator animator,
                         CharacterController controller,
                         FSMManager fsmManager,
                         InputSystem inputSystem) : base(EChangeType.e2DBlendTree, transform, animator, fsmManager)
        {
            this.controller = controller;

            this.inputSystem = inputSystem;

            // 运动的动画可以随时切换到其他动作
            this.canChange = true;
        }


        public override void OnStart()
        {
            // Trigger 切换到Move 动作
            animator.SetTrigger(AnimatorEnum.Anim_TMove);

            // 控制速度
            animator.SetFloat(AnimatorEnum.Anim_F_MoveForward, forward);
            animator.SetFloat(AnimatorEnum.Anim_F_MoveRight, right);
        }

        public override void OnUpdate()
        {
            // TODO 这种方式获取的值变化太突然，导致动作切换太突兀，需要Lerp平缓化，使动作融合更舒服
            forward = inputSystem.GetVertical();     // 前后速度
            right = inputSystem.GetHorizontal();     // 左右速度

            // 根据输入获取速度
            speed = inputSystem.GetMoveSpeed();
            forward *= speed;
            right *= speed;

            moveDirection = new Vector3(forward, 0, right);
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;

            // 更新Animator 动画参数
            animator.SetFloat(AnimatorEnum.Anim_F_MoveForward, forward);
            animator.SetFloat(AnimatorEnum.Anim_F_MoveRight, right);

            // 设置Controller
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