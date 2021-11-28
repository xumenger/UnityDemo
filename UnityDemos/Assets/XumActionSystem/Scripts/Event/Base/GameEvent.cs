namespace xum.action
{
    public abstract class GameEvent
    {

        /// <summary>
        /// 返回这个事件对应的事件字典中key
        ///
        /// 事件字典中的key 是这个枚举
        /// 而不是事件对象
        /// 因为每次发布事件的时候会创建一个新的事件对象
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract GameEventEnum getEventEnum();
        
    }

}