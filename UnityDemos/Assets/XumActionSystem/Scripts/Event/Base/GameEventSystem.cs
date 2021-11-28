using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    /**
     * Add in 2021-09-21
     * 游戏事件分发模块
     * 
     * 该脚本需要由开发者显式挂载到游戏场景中同名的游戏物体中
     * 
     * 常见的事件包括
     * 1. 用户输入事件          => 触发玩家运动、攻击等
     * 2. 敌人的攻击            => 触发玩家受伤
     * 3. 玩家走到墙前面        => 触发玩家开始攀爬
     * 4. 玩家走到水前          => 触发玩家开始游泳
     * 5. 玩家走到齐腰矮墙前     => 触发玩家跳过矮墙
     * 6. 
     * 
     */
    public abstract class GameEventSystem
    {
        // eventDict 是所有GameEventSystem 子类共享的全局变量。不考虑多线程并发环境
        static private Dictionary<GameEventEnum, List<GameEventAction>> eventDict = new Dictionary<GameEventEnum, List<GameEventAction>>();


        /// <summary>
        /// 发布事件
        /// 
        /// </summary>
        /// <param name="gameEvent"></param>
        public void publicEvent(GameEvent gameEvent)
        {
            // 如果事件字典中没有这个事件，直接返回
            if (!eventDict.ContainsKey(gameEvent.getEventEnum()))
            {
                return;
            }

            // 找到事件字典对应的处理类列表，回调所有
            List<GameEventAction> actionList = eventDict[gameEvent.getEventEnum()];
            foreach (GameEventAction eventAction in actionList)
            {
                eventAction.doAction(gameEvent);
            }
        }


        /// <summary>
        /// 注册事件和事件处理类
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="eventAction"></param>
        public void registerEventAction(GameEventEnum eventEnum, GameEventAction eventAction)
        {
            List<GameEventAction> actionList = null;

            if (!eventDict.ContainsKey(eventEnum))
            {
                eventDict.Add(eventEnum, new List<GameEventAction>());
            }

            actionList = eventDict[eventEnum];
            actionList.Add(eventAction);
        }
    }
}