using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    /// <summary>
    /// FSMManager 继承GameEventSystem
    /// 可以注册事件处理类
    /// 也可以发布事件触发处理类回调
    /// 
    /// </summary>
    public class FSMManager : GameEventSystem
    {
        // 游戏物体的组件
        protected GameObject gameObject;
        protected MonoBehaviour behaviour;
        protected CharacterController controller;
        protected Animator animator;

        // 输入系统
        protected InputSystem inputSystem;


        // FSM动画状态相关
        FSMState lastState = null;    // 上一个状态
        FSMState curState = null;     // 当前状态

        // 所有动作状态类字典
        public Dictionary<StateEnum, FSMState> allStateDict;


        /// <summary>
        /// 构造方法
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="behaviour"></param>
        /// <param name="controller"></param>
        /// <param name="animator"></param>
        /// <param name="inputSystem"></param>
        /// <param name="stateId"></param>
        public FSMManager(GameObject gameObject,
                          MonoBehaviour behaviour,
                          CharacterController controller,
                          Animator animator,
                          InputSystem inputSystem,
                          StateEnum stateId)
        {
            this.gameObject = gameObject;
            this.behaviour = behaviour;
            this.controller = controller;
            this.animator = animator;

            this.inputSystem = inputSystem;

            this.allStateDict = new Dictionary<StateEnum, FSMState>();

            // 初始化角色的状态
            InitAllFSMState();

            // 切换到Move 状态作为初始状态
            ChangeToState(stateId);
        }


        /// <summary>
        /// Update is called once per frame
        /// 
        /// </summary>
        public void OnUpdate()
        {
            if (null != curState)
            {
                curState.OnUpdate();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void OnFixedUpdate()
        {
            if (null != curState)
            {
                curState.OnFixedUpdate();
            }
        }


        /// <summary>
        /// 用于方便在每个具体的状态类中做IK
        /// 提供该接口
        /// 以PlayerController 为例子
        /// PlayerController.OnAnimatorIK() 调用FSMManager.OnAnimatorIK()
        /// FSMManager.OnAnimatorIK 则调用具体状态类的OnAnimatorIK()
        /// 
        /// </summary>
        /// <param name="layerIndex"></param>
        public void OnAnimatorIK(int layerIndex)
        {
            if (null != curState)
            {
                curState.OnAnimatorIK(layerIndex);
            }
        }


        /// <summary>
        /// 初始化角色的状态，由各个子类自己实现
        /// 
        /// </summary>
        public virtual void InitAllFSMState()
        {

        }


        /// <summary>
        /// 状态切换
        /// 
        /// </summary>
        /// <param name="stateId"></param>
        public void ChangeToState(StateEnum stateId)
        {
            // 根据状态ID 获取状态
            FSMState state = allStateDict[stateId];

            // 如果上一个当前动作不允许切换，则先退出等待当前动画播放完
            if ((null != curState) && !curState.CanChange)
            {
                return;
            }

            // 更新状态
            lastState = curState;
            curState = state;

            // 切换状态
            if (null != lastState)
            {
                lastState.OnEnd();
            }
            curState.OnStart();
        }


        /// <summary>
        /// 切换到上一个状态
        /// 
        /// </summary>
        public void ChangeToLastState()
        {
            // 如果上一个当前动作不允许切换，则先退出等待当前动画播放完
            if ((null != curState) && !curState.CanChange)
            {
                return;
            }

            if (null != lastState)
            {
                FSMState tmpState = lastState;
                lastState = curState;
                curState = tmpState;
            }

            // 切换状态
            if (null != lastState)
            {
                lastState.OnEnd();
            }
            curState.OnStart();

        }


        /// <summary>
        /// 必须MonoBehaviour 才有StartCoroutine() 协程方法
        /// 
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return this.behaviour.StartCoroutine(enumerator);
        }


        /// <summary>
        /// Debug 输出
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void DebugLog(string msg)
        {
            Debug.Log(msg);
        }
    }
}

