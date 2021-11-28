using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{

    /// <summary>
    /// Add in 2021-09-21
    /// 敌人AI输入模块
    /// 
    /// </summary>
    public class EnemyAIInputSystem : InputSystem
    {
        private bool isMove = false;

        public EnemyAIInputSystem() : base()
        {

        }


        /// <summary>
        /// 判断是否运动
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsMove()
        {
            if (!isMove)
            {
                isMove = true;
                return true;
            }

            return false;
        }


        /// <summary>
        /// 判断向前
        /// 
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetVertical()
        {
            return new Vector3(0.8f, 0f, 0f);
        }


        /// <summary>
        /// 判断向右
        /// 
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetHorizontal()
        {
            return Vector3.zero;
        }
    }
}