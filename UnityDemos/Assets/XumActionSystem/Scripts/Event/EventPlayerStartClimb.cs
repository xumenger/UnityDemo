using UnityEngine;
using System.Collections;

namespace xum.action
{
    /// <summary>
    /// Add in 2021-11-22
    /// 
    /// 判断玩家是否走到墙边、山崖边等以触发攀爬的事件
    ///
    /// 
    /// </summary>
    public class EventPlayerStartClimb : GameEvent
    {
        private GameObject player;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="player"></param>
        public EventPlayerStartClimb(GameObject player)
        {
            this.player = player;
        }

        /// <summary>
        /// 返回这个事件对应的事件字典中key
        /// 
        /// </summary>
        /// <returns></returns>
        public override GameEventEnum getEventEnum()
        {
            return GameEventEnum.eStartClimb;
        }


        /// <summary>
        /// 获取PlayerController 脚本
        ///
        /// 这个设计思路是在Event 类中返回Action 需要的静态信息
        /// 
        /// </summary>
        /// <returns></returns>
        public PlayerController getPlayerController()
        {
            return player.GetComponent<PlayerController>();
        }

    }
}