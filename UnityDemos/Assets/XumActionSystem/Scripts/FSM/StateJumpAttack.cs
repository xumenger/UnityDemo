using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class StateJumpAttack : FSMState
    {
        CharacterController controller;


        public StateJumpAttack(GameObject gameObject,
                               Animator animator,
                               CharacterController controller,
                               FSMManager fsmManager) : base(EChangeType.eTrigger, gameObject, animator, fsmManager)
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