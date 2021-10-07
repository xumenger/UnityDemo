using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    public class PlayerKeyMouseInputSystem : InputSystem
    {
        // 判断是否发起攻击
        public override bool IsAttack()
        {
            // 【鼠标左键】攻击
            return Input.GetMouseButtonDown(0);
        }

        // 判断是否跳跃
        public override bool IsJump()
        {
            // 【Space】跳跃
            return Input.GetKeyDown(KeyCode.Space);
        }

        // 判断是否运动
        public override bool IsMove()
        {
            // 【WASD】运动
            return (Input.GetKeyDown(KeyCode.W) ||
                    Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) ||
                    Input.GetKeyDown(KeyCode.D));
        }

        // 判断向前
        public override float GetVertical()
        {
            return Input.GetAxis("Vertical");
        }

        // 判断向右
        public override float GetHorizontal()
        {
            return Input.GetAxis("Horizontal");
        }

        public override float GetMoveSpeed()
        {
            float speed = 1.0f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 2.0f;
            }

            return speed;
        }


        // 判断是否踢腿
        public override bool IsKick()
        {
            // 【鼠标中键】攻击
            return Input.GetMouseButtonDown(2);
        }
    }
}