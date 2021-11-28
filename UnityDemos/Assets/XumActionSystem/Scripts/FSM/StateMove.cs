using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    /// <summary>
    /// 使用Blend Tree 将Idle、Walk、Run 等动作进行封装
    /// 
    /// </summary>
    public class StateMove : FSMState
    {
        InputSystem inputSystem;

        CharacterController controller;

        public float speed;
        private Vector3 desiredMoveDirection = Vector3.zero;

        public float desiredRotationSpeed = 0.01f;

        private Vector3 InputX;
        private Vector3 InputZ;


        public StateMove(GameObject gameObject,
                         Animator animator,
                         CharacterController controller,
                         FSMManager fsmManager,
                         InputSystem inputSystem) : base(EChangeType.e2DBlendTree, gameObject, animator, fsmManager)
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
            animator.SetFloat(AnimatorEnum.Anim_F_MoveForward, InputZ.magnitude);
            animator.SetFloat(AnimatorEnum.Anim_F_MoveRight, InputX.magnitude);
        }


        public override void OnUpdate()
        {
            // TODO 这种方式获取的值变化太突然，导致动作切换太突兀，需要Lerp平缓化，使动作融合更舒服
            InputX = inputSystem.GetHorizontal();     // 左右速度
            InputZ = inputSystem.GetVertical();       // 前后速度


            // 根据输入获取速度
            speed = inputSystem.GetMoveSpeed();

            desiredMoveDirection = InputZ + InputX;

            desiredMoveDirection *= speed;

            // 更新Animator 动画参数
            animator.SetFloat(AnimatorEnum.Anim_F_MoveForward, InputZ.magnitude);
            animator.SetFloat(AnimatorEnum.Anim_F_MoveRight, InputX.magnitude);

            // 设置Controller 移动
            controller.Move(desiredMoveDirection * Time.deltaTime);

            // 通过鼠标控制角色旋转
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);

        }

        public override void OnEnd()
        {

        }

        public override void DoEvent(object param)
        {

        }
    }

}