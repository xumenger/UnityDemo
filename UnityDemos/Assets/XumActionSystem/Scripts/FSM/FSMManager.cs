using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class FSMManager
    {
        // 游戏物体的组件
        protected Transform transform;
        protected MonoBehaviour behaviour;
        protected CharacterController controller;
        protected Animator animator;

        protected InputSystem inputSystem;


        // FSM动画状态相关
        FSMState lastState = null;    // 上一个状态
        FSMState curState = null;     // 当前状态

        public Dictionary<StateEnum, FSMState> allStateDict;

        public FSMManager(Transform transform,
                          MonoBehaviour behaviour,
                          CharacterController controller,
                          Animator animator,
                          InputSystem inputSystem,
                          StateEnum stateId)
        {
            this.transform = transform;
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


        // Update is called once per frame
        public void OnUpdate()
        {
            if (null != curState)
            {
                curState.OnUpdate();
            }
        }


        // 初始化角色的状态，由各个子类自己实现
        public virtual void InitAllFSMState()
        {

        }


        // 状态切换
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


        // 切换到上一个状态
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


        // 必须MonoBehaviour 才有StartCoroutine() 协程方法
        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return this.behaviour.StartCoroutine(enumerator);
        }


        // Debug 输出
        public void DebugLog(string log)
        {
            Debug.Log(log);
        }
    }
}

