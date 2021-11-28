namespace xum.action
{
    public abstract class GameEvent
    {

        /**
         * 获取这个事件类对应的枚举
         * 事件字典中的key 是这个枚举
         * 而不是事件对象
         * 因为每次发布事件的时候会创建一个新的事件对象
         */
        public virtual GameEventEnum getEventEnum()
        {
            return GameEventEnum.eNULL;
        }
        
    }

}