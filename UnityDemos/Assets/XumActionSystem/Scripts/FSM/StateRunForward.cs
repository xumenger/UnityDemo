using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace xum.action
{
    public class StateRunForward : FSMState
    {
        public StateRunForward(GameObject gameObject,
                               Animator animator,
                               FSMManager fsmManager) : base(EChangeType.eTrigger, gameObject, animator, fsmManager)
        {

        }

        public override void OnStart()
        {
            // 播放“RunForward”动画
            animator.SetTrigger(AnimatorEnum.Anim_TRunForward);

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
