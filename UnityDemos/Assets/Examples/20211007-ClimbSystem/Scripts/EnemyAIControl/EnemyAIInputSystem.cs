using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
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
        public override float GetVertical()
        {
            return 0.8f;
        }

        // 判断向右
        public override float GetHorizontal()
        {
            return 0.0f;
        }
    }
}