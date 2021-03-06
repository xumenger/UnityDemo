using UnityEngine;


namespace xum.action
{
    /// <summary>
    /// Add in 2021-09-21
    /// 对于“用户输入系统”的封装
    /// 
    /// </summary>
    public class InputSystem
    {
        public InputSystem()
        {

        }

        // 判断是否发起攻击
        public virtual bool IsAttack()
        {
            return false;
        }

        // 判断是否跳跃
        public virtual bool IsJump()
        {
            return false;
        }

        // 判断是否运动
        public virtual bool IsMove()
        {
            return false;
        }

        // 获取垂直方向移动值
        public virtual Vector3 GetVertical()
        {
            return Vector3.zero;
        }

        // 获取水平方向移动值
        public virtual Vector3 GetHorizontal()
        {
            return Vector3.zero;
        }

        // 获取移动速度
        public virtual float GetMoveSpeed()
        {
            return 1.0f;
        }

        // 判断是否踢腿
        public virtual bool IsKick()
        {
            return false;
        }
    }

}