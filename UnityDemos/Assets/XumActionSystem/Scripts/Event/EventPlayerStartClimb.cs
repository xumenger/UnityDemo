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
        // 事件的原则：保存事件发生时尽可能多的上下文信息
        private GameObject player;      // 玩家对象
        private GameObject obstacle;    // 障碍物对象
        private Vector3 hitNormal;      // 障碍物的表面法线，用于调整玩家的朝向


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="player"></param>
        public EventPlayerStartClimb(GameObject player, GameObject obstacle, Vector3 hitNormal)
        {
            this.player = player;
            this.obstacle = obstacle;
            this.hitNormal = hitNormal;
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


        /// <summary>
        /// 获取玩家游戏对象
        /// 
        /// </summary>
        /// <returns></returns>
        public GameObject getPlayer()
        {
            return this.player;
        }


        /// <summary>
        /// 获取障碍物游戏对象
        /// 
        /// </summary>
        /// <returns></returns>
        public GameObject getObstacle()
        {
            return this.obstacle;
        }


        /// <summary>
        /// 获取障碍物的表面法线
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector3 getHitNormal()
        {
            return this.hitNormal;
        }

    }
}