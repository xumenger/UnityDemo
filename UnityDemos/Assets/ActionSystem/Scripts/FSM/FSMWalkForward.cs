using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class FSMWalkForward : FSMState
    {
        public FSMWalkForward(Transform transform,
                              Animator animator,
                              FSMManager fsmManager) : base(EChangeType.eTrigger, transform, animator, fsmManager)
        {

        }

        public override void OnStart()
        {
            // 播放“WalkForward”动画
            animator.SetTrigger(AnimatorEnum.Anim_TWalkForward);

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