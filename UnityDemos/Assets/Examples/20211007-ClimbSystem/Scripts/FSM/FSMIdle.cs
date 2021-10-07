using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    public class FSMIdle : FSMState
    {
        public FSMIdle(Transform transform,
                       Animator animator,
                       FSMManager fsmManager) : base(EChangeType.eTrigger, transform, animator, fsmManager)
        {

        }

        public override void OnStart()
        {
            // 播放“Idle”动画
            animator.SetTrigger(AnimatorEnum.Anim_TIdle);

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