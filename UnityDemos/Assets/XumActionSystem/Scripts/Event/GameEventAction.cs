namespace xum.action
{
    public abstract class GameEventAction
    {
        protected GameEventSystem gameEventSystem;
        protected GameEvent gameEvent;

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
            this.gameEventSystem.registerEventAction(gameEvent, this);
        }

        /// <summary>
        /// 其对应的事件发生后，对应回调该方法处理事件
        /// </summary>
        public virtual void doAction()
        {
            
        }
    }

}