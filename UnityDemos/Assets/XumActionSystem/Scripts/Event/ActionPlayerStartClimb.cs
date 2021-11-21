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
        public ActionPlayerStartClimb(GameEventSystem gameEventSystem,
                                      GameEvent gameEvent) : base(gameEventSystem, gameEvent)
        {
            
        }

        /// <summary>
        /// 判断玩家开始攀爬事件后的处理
        /// 调用PlayerController 切换到攀爬状态
        /// </summary>
        public override void doAction()
        {
            EventPlayerStartClimb eventPlayerStartClimb = (EventPlayerStartClimb)gameEvent;

            eventPlayerStartClimb.getPlayerController().ChangeToState(StateEnum.eClimb);
        }
    }

}