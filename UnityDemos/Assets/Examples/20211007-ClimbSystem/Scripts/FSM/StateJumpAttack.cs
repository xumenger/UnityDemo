using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    public class StateJumpAttack : FSMState
    {
        CharacterController controller;


        public StateJumpAttack(Transform transform,
                               Animator animator,
                               CharacterController controller,
                               FSMManager fsmManager) : base(EChangeType.eTrigger, transform, animator, fsmManager)
        {
            this.controller = controller;
        }


        public override void OnStart()
        {
            // 播放“JumpAttack”动画
            animator.SetTrigger(AnimatorEnum.Anim_T_JumpAttack);
        }

        public override void OnUpdate()
        {

        }

        public override void OnEnd()
        {

        }

        public override void DoEvent(object param)
        {

        }
    }

}