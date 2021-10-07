using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    // 动画切换的方式
    public enum EChangeType
    {
        eNULL = -1,
        eTrigger,       // Trigger 触发
        eBoolean,       // Boolean 变量切换
        eFloat,         // Float 变量切换
        eInteger,       // Integer 变量切换
        eBlendTree,     // BlendTree 混合树动画
        e2DBlendTree,   // 2D Blend Tree 2D混合树动画
        eSize,
    }

    public abstract class FSMState 
    {
        protected EChangeType changeType;        // 动画控制、切换的方式

        protected bool canChange;                // 是否可以切换
        public bool CanChange => (canChange);

        protected Transform transform;           // 游戏对象
        protected Animator animator;             // 动画控制器

        protected FSMManager fsmManager;         // 有限状态管理


        public FSMState(EChangeType changeType,
                        Transform transform,
                        Animator animator,
                        FSMManager fsmManager)
        {
            this.changeType = changeType;
            canChange = false;

            this.transform = transform;
            this.animator = animator;
            this.fsmManager = fsmManager;
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnEnd()
        {

        }

        public virtual void DoEvent(object param)
        {

        }


        /**
         * 协程：等待本动作完成后，切换到上一个状态
         * 适用于Jump、Attack、Kick 等播放一次的动作
         */
        protected IEnumerator WaitForAWhile(float durationTime)
        {
            yield return new WaitForSeconds(durationTime);

            this.canChange = true;

            fsmManager.ChangeToLastState();

            this.canChange = false;
        }
    }
}
