参考[https://github.com/mixandjam/Batman-Arkham-Combat](https://github.com/mixandjam/Batman-Arkham-Combat)

思考这几个点

1. 用哪些基础动作
2. 攻击者与被攻击者如何配合
3. 反思别人的动画状态机是怎么设计的
4. 脚本编写用到了哪些技术
5. 用到了哪些特效、音效
6. 如何用状态机模式崇高

## 动画资源与配置

模型使用我找的其他的模型

这个项目中的动画资源想用于ybot 还不能直接用，根据这个项目用到的动画，对应去mixmo 找相应的动画，放在Combat Pack 文件夹中

* Head Hit：头部被击中
* Dodging：类似拳击中的闪躲动作
* Cross Punch: 斜冲拳
* Punching
* Crescent Kick/Inside Crescent：跳踢
* Flip Kick：向前翻转后踢腿
* Flying Knee Punch：
* Flying Kick：
* Flying Back Death：被踢飞后死亡
* Fight Idle/Bouncing Fight/Mma Idle：站立时，抬拳防守状态
* Walking：各种行走动作
* Running：各种跑步动作
* Walking Backwards：呈防守式后撤

一些技巧

* 通过调整Animator 中每个状态的speed 来调整动画效果
* Blend Tree 实现动画混合

首先参考Batman-Arkham-Combat 项目的EnemyAnimator、AnimatorController_Jamo，复刻对方的动画状态机、好好理解其中的每个动作、变量

在配置动画的过程中可能遇到一些问题，整理了解决方案：[好好玩游戏：Copy From Other Avatar 报错解决方案](http://www.xumenger.com/unity-animator-avatar-20210925/)

## Cinemachine

【Cinemachine】=>【Create Virtual Camera】，进行如下Follow、Look At 等设置

![](./image/01.png)

参考[好好玩游戏：Cinemachine 实现角色跟随](http://www.xumenger.com/cine-machine-20210914/)、[好好玩游戏：Cinemachine 与Timeline 实现镜头融合](http://www.xumenger.com/cine-machine-20210915/) 调整至合适的视角

