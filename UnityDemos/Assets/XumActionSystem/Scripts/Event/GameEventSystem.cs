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
     */
    public class GameEventSystem : MonoBehaviour
    {
        Dictionary<GameEvent, List<GameEventAction>> eventDict = new Dictionary<GameEvent, List<GameEventAction>>();

        /// <summary>
        /// Update is called once per frame
        ///
        /// 这个有点类似事件驱动模型
        /// 但是每次需要遍历所有注册的事件
        /// 假如后续随着项目体量变大，事件有10000个，每一帧需要遍历所有的事件
        /// 但实际其实每一帧事件往往只会发生10个
        /// 那么就会有9990次没有必要的遍历，浪费计算量，影响性能
        /// 关于这一点需要考虑怎么优化！！
        /// 
        /// </summary>
        void Update()
        {
            // 在每一帧里遍历所有的事件
            foreach(KeyValuePair<GameEvent, List<GameEventAction>> eventKV in eventDict)
            {
                GameEvent gameEvent = eventKV.Key;

                // 判断当前帧事件是否发生
                if (gameEvent.isHappened())
                {
                    // 如果事件发生了，则对应所有的事件处理类被回调
                    List<GameEventAction> actionList = eventKV.Value;
                    foreach (GameEventAction eventAction in actionList)
                    {
                        eventAction.doAction();
                    }
                }
            }
        }


        /// <summary>
        /// 注册事件和事件处理类
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="eventAction"></param>
        public void registerEventAction(GameEvent gameEvent, GameEventAction eventAction)
        {
            List<GameEventAction> actionList = eventDict[gameEvent];
            if (null == actionList)
            {
                actionList = new List<GameEventAction>();
                eventDict[gameEvent] = actionList;
            }

            actionList.Add(eventAction);
        }
    }
}