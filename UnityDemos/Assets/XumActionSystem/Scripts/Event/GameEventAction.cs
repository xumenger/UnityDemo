using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class GameEventAction
    {
        private GameEventSystem gameEventSystem;
        private GameEvent gameEvent;

        /// <summary>
        /// 在事件处理类构造的时候完成注册
        /// </summary>
        /// <param name="gameEventSystem"></param>
        /// <param name="gameEvent"></param>
        public GameEventAction(GameEventSystem gameEventSystem,
                               GameEvent gameEvent)
        {
            this.gameEventSystem = gameEventSystem;
            this.gameEvent = gameEvent;

            // 注册到事件管理器中
            gameEventSystem.registerEventAction(gameEvent, this);
        }

        /// <summary>
        /// 其对应的事件发生后，对应回调该方法处理事件
        /// </summary>
        public void doAction()
        {
            
        }
    }

}