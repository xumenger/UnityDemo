using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    /**
     * 动作状态枚举
     */
    public enum StateEnum
    {
        eNULL = -1,
        eMove,              // 运动：Idle、走、跑
        eIdle,              // 静止状态
        eWalkForward,       // 向前走
        eRunForward,        // 向前跑
        eJumpUp,            // 向上跳
        eJumpForward,       // 向前跳
        eDeathForward,      // 向前倒前死掉
        eDeathBack,         // 向后倒下死掉
        eKick,              // 抬右脚踢
        ePowerUp,           // 向天怒吼发大招
        eJumpAttack,        // 跳起来攻击
        eFistAttack,        // 拳头攻击
        eSlashRightHand,    // 右手挥动武器攻击
        eClimb,             // 攀爬
        eSize,
    }


    /**
     * 在Animator 动画状态机中的变量
     */
    public class AnimatorEnum
    {
        // Trigger，移动
        public static string Anim_TMove = "Base Layer.move";

        // Float，前后移动
        public static string Anim_F_MoveForward = "Base Layer.fMoveForward";

        // Float，左右移动
        public static string Anim_F_MoveRight = "Base Layer.fMoveRight";

        // Trigger，站立
        public static string Anim_TIdle = "Base Layer.idle";

        // Trigger，向前走
        public static string Anim_TWalkForward = "Base Layer.walk forward";

        // Trigger，向前跑
        public static string Anim_TRunForward = "Base Layer.run forward";

        // Trigger，向上跳
        public static string Anim_T_JumpUp = "Base Layer.jump up";

        // Trigger，向前跳
        public static string Anim_T_JumpForward = "Base Layer.jump forward";

        // Trigger，向前倒前死掉
        public static string Anim_T_DeathForward = "Base Layer.death forward";

        // Trigger，向后倒下死掉
        public static string Anim_T_DeathBack = "Base Layer.death back";

        // Trigger，抬右脚踢
        public static string Anim_T_Kick = "Base Layer.kick";

        // Trigger，向天怒吼发大招
        public static string Anim_T_PowerUp = "Base Layer.power up";

        // Trigger，跳起来攻击
        public static string Anim_T_JumpAttack = "Base Layer.jump attack";

        // Trigger，拳头攻击
        public static string Anim_T_FistAttack = "Base Layer.fist attack";

        // Trigger，右手挥动武器攻击
        public static string Anim_T_SlashRightHand = "Base Layer.slash right hand";

        // Trigger，攀爬
        public static string Anim_T_Climb  = "Base Layer.climb";

        // Float，水平左右爬的速度
        public static string Anim_F_ClimbHorizontal = "Base Layer.fClimbHorizontal";

        // Float，垂直上下爬的速度
        public static string Anim_F_ClimbVertical = "Base Layer.fClimbVertical";

    }
}