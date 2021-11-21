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

        // 射线长度
        public float wallRayLength = 1;


        public EventPlayerStartClimb(GameObject player)
        {
            this.player = player;
        }

        /// <summary>
        /// 通过射线判断判断玩家是否走到墙边
        /// </summary>
        /// <returns></returns>
        public override bool isHappened()
        {
            Vector3 origin = player.transform.position;
            Vector3 dir = player.transform.forward;

            // 使用射线检测是否走到墙边，hit 表示射线命中墙的位置
            RaycastHit hit;
            Debug.DrawRay(origin, dir * wallRayLength, Color.yellow);
            if (Physics.Raycast(origin, dir, out hit, wallRayLength))
            {
                return true;
            }

            return false;
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