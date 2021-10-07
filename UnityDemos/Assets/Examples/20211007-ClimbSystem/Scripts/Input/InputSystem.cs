using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace example.y20211007
{
    /**
     * Add in 2021-09-21
     * 对于“用户输入系统”的封装
     * 
     */
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

        public virtual float GetVertical()
        {
            return 0.0f;
        }

        public virtual float GetHorizontal()
        {
            return 0.0f;
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