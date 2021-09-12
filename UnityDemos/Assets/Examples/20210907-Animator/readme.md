添加一个Virtual Camera，将Follow、Look At 都设置为【ybot】

在【ybot】的mixamorig:RightHandThumb1 下添加一个Cylinder 作为玩家的武器，调节其位置、旋转，使其与角色看起来更符合视觉效果

取消勾选【ybot】Animator 组件上的Apply Root Motion 选项

使用的动画资源来自mixamo

* Male Locomotion Pack
* Pro Sword and Shield Pack

动画状态机中，使用一个2D Blend Tree，来实现idle、前行、后行、前跑、后跑、左行、右行、左跑、右跑的融合

![](./images/01.png)

所有的进行如下配置，否则实际运动的方向、动作循环效果是不正确的

* 勾选Loop Time
* Root Transform Rotation 勾选Back Into Pose、Based Upon 选择Original

所有动作的Source 都选择对应模型的Avatar（这块的影响是什么？）

![](./images/02.png)

## 想要哪些动作效果

基础动作

* WASD 按键控制角色的前后左右移动
* Space 键控制角色跳跃
* K 按键控制角色的攻击

复杂的动作

* 

## 常见问题以及解决方案

在2D Blend Tree 中，可能会出现如下的“鬼畜”现象，两个动作的融合效果并不好

![](./images/01-01.gif)

>解决方案：

使用Trigger 触发jump forward 这个动画播放的时候，看到角色的脚乱掉了

![](./images/01-02.gif)

>解决方案：