namespace xum.action
{
    /// <summary>
    /// 事件触发时的回调处理类
    /// 
    /// </summary>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public GameEventEnum getEventEnum()
        {
            return eventEnum;
        }


        /// <summary>
        /// 其对应的事件发生后，对应回调该方法处理事件
        /// </summary>
        public virtual void doAction(GameEvent gameEvent)
        {
            
        }
    }

}