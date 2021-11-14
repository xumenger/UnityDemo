using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{

    /**
     * Add in 2021-09-21
     * 敌人AI输入模块
     * 
     */
    public class EnemyAIInputSystem : InputSystem
    {
        private bool isMove = false;

        public EnemyAIInputSystem() : base()
        {

        }


        // 判断是否运动
        public override bool IsMove()
        {
            if (!isMove)
            {
                isMove = true;
                return true;
            }

            return false;
        }

        // 判断向前
        public override Vector3 GetVertical()
        {
            return new Vector3(0.8f, 0f, 0f);
        }

        // 判断向右
        public override Vector3 GetHorizontal()
        {
            return Vector3.zero;
        }
    }
}