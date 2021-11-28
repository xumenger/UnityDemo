using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    /// <summary>
    /// Add in 2021-09-21
    /// 游戏事件分发模块
    ///
    /// 该脚本需要由开发者显式挂载到游戏场景中同名的游戏物体中
    ///
    /// 常见的事件包括比如
    /// 1. 用户输入事件          => 触发玩家运动、攻击等
    /// 2. 敌人的攻击            => 触发玩家受伤
    /// 3. 玩家走到墙前面        => 触发玩家开始攀爬
    /// 4. 玩家走到水前          => 触发玩家开始游泳
    /// 5. 玩家走到齐腰矮墙前     => 触发玩家跳过矮墙
    /// 6.
    /// 
    /// </summary>
    public abstract class GameEventSystem
    {
        /// <summary>
        /// eventDict 是所有GameEventSystem 子类共享的全局变量。不考虑多线程并发环境
        /// 
        /// </summary>
        static private Dictionary<GameEventEnum, HashSet<GameEventAction>> eventDict = new Dictionary<GameEventEnum, HashSet<GameEventAction>>();


        /// <summary>
        /// 发布事件
        /// 
        /// </summary>
        /// <param name="gameEvent"></param>
        public void publishEvent(GameEvent gameEvent)
        {
            // 如果事件字典中没有这个事件，直接返回
            if (!eventDict.ContainsKey(gameEvent.getEventEnum()))
            {
                return;
            }

            // 找到事件字典对应的处理类列表，回调所有
            HashSet<GameEventAction> actionSet = eventDict[gameEvent.getEventEnum()];
            foreach (GameEventAction eventAction in actionSet)
            {
                eventAction.doAction(gameEvent);
            }
        }


        /// <summary>
        /// 注册事件和事件处理类
        /// 
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="eventAction"></param>
        public void registerEventAction(GameEventAction eventAction)
        {
            HashSet<GameEventAction> actionSet = null;

            if (!eventDict.ContainsKey(eventAction.getEventEnum()))
            {
                eventDict.Add(eventAction.getEventEnum(), new HashSet<GameEventAction>());
            }

            actionSet = eventDict[eventAction.getEventEnum()];
            actionSet.Add(eventAction);
        }


        /// <summary>
        /// 移除事件
        /// 
        /// </summary>
        /// <param name="eventAction"></param>
        /// <returns></returns>
        public bool removeEventAction(GameEventAction eventAction)
        {
            HashSet<GameEventAction> actionSet = null;

            if (!eventDict.ContainsKey(eventAction.getEventEnum()))
            {
                return false;
            }

            actionSet = eventDict[eventAction.getEventEnum()];
            actionSet.Remove(eventAction);

            return true;
        }
    }
}