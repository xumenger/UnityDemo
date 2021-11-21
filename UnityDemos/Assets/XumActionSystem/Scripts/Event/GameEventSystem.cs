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
    public class GameEventSystem : MonoBehaviour
    {
        private Dictionary<GameEvent, List<GameEventAction>> eventDict = new Dictionary<GameEvent, List<GameEventAction>>();


        // 玩家游戏对象，这个需要开发者将玩家在Unity拖动到这个属性上，这个方式不太好，后续优化
        public GameObject Player;


        /// <summary>
        /// 在这里初始化所有的事件和事件处理类
        ///
        /// 这种编码方式有一个缺点，就是后续每新增一个事件，就需要在这里修改代码！
        /// 后续参考Spring 的依赖注入来优化
        /// 
        /// </summary>
        void Start()
        {
            // 注册玩家开始攀爬的事件及其方法
            EventPlayerStartClimb eventPlayerStartClimb = new EventPlayerStartClimb(Player);
            ActionPlayerStartClimb actionPlayerStartClimb = new ActionPlayerStartClimb(this, eventPlayerStartClimb);

            // 

        }


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
        /// 再说一些具体的情况，比如有1000 个事件是通过射线检测判断是否发生的话
        /// 那么一个帧里面的事件循环就会触发1000 次射线检测
        /// 这种性能感觉就会是很大的问题
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
            List<GameEventAction> actionList = null;

            if (!eventDict.ContainsKey(gameEvent))
            {
                eventDict.Add(gameEvent, new List<GameEventAction>());
            }

            actionList = eventDict[gameEvent];
            actionList.Add(eventAction);
        }
    }
}