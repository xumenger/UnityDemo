using UnityEngine;
using System.Collections;

namespace xum.action
{
    /// <summary>
    /// Add in 2021-11-22
    /// 
    /// 判断玩家是否走到墙边、山崖边等以触发攀爬的事件处理
    /// 
    /// </summary>
    public class ActionPlayerStartClimb : GameEventAction
    {
        public ActionPlayerStartClimb(GameEventEnum eventEnum) : base(eventEnum)
        {
            
        }


        /// <summary>
        /// 判断玩家开始攀爬事件后的处理
        /// 调用PlayerController 切换到攀爬状态
        /// 
        /// </summary>
        public override void doAction(GameEvent gameEvent)
        {
            // 开始攀爬事件类型转换
            EventPlayerStartClimb eventPlayerStartClimb = (EventPlayerStartClimb)gameEvent;

            // 调整玩家朝向，朝向障碍物。设置与障碍物的表面的法向量相反即可
            eventPlayerStartClimb.getPlayer().transform.forward = -eventPlayerStartClimb.getHitNormal();

            // 切换到攀爬动作
            eventPlayerStartClimb.getPlayerController().ChangeToState(StateEnum.eClimb);
        }
    }

}