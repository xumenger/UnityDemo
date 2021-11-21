using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
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


        /// <summary>
        /// 从A状态切换到B状态时
        /// 会先执行A的OnEnd() 方法
        /// 然后执行B的OnStart() 方法
        /// </summary>
        public virtual void OnStart()
        {

        }

        /// <summary>
        /// 如果正处在当前状态
        /// 则OnUpdate() 方法逐帧回调
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFixedUpdate()
        {

        }

        /// <summary>
        /// 从A状态切换到B状态时
        /// 会先执行A的OnEnd() 方法
        /// 然后执行B的OnStart() 方法
        /// </summary>
        public virtual void OnEnd()
        {

        }

        public virtual void DoEvent(object param)
        {

        }

        /// <summary>
        /// 用于方便在每个具体的状态类中做IK
        /// 提供该接口
        /// 以PlayerController 为例子
        /// PlayerController.OnAnimatorIK() 调用FSMManager.OnAnimatorIK()
        /// FSMManager.OnAnimatorIK 则调用具体状态类的OnAnimatorIK()
        /// </summary>
        /// <param name="layerIndex"></param>
        public virtual void OnAnimatorIK(int layerIndex)
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
