namespace xum.action
{
    public abstract class GameEventAction
    {
        protected GameEventEnum eventEnum;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameEvent"></param>
        public GameEventAction(GameEventEnum eventEnum)
        {
            this.eventEnum = eventEnum;
        }

        /// <summary>
        /// 其对应的事件发生后，对应回调该方法处理事件
        /// </summary>
        public virtual void doAction(GameEvent gameEvent)
        {
            
        }
    }

}