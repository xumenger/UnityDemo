using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class StateSlashRightHand : FSMState
    {
        // TODO 以下变量实现配置化
        public float durationTime = 0.8f;      // 该动作只应该播放一次，持续时间

        public StateSlashRightHand(GameObject gameObject,
                                   Animator animator,
                                   FSMManager fsmManager) : base(EChangeType.eTrigger, gameObject, animator, fsmManager)
        {

        }


        public override void OnStart()
        {
            // 播放“JumpAttack”动画
            animator.SetTrigger(AnimatorEnum.Anim_T_SlashRightHand);

            // 设置该动作的持续时间后切换到上一个状态
            fsmManager.StartCoroutine(WaitForAWhile(durationTime));
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