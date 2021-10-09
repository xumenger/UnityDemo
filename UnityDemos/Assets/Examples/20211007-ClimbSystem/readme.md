## 一些遗留问题

1. 如何使用状态机模式对这个攀爬的动作实现进行重构
2. 如何基于射线判断使得玩家的位置贴合到墙上
3. 好好研究清楚Unity 的IK ！

## 参考一个视频教程实现的攀爬系统

参考[荒野之息攀爬系统 Botw Climbing - Unity Tutorial](https://www.bilibili.com/video/BV1F4411T722)

参考[好好玩游戏：Unity 切换输入系统](http://www.xumenger.com/switch-input-system-20211007/) 进行Input Manager 与Input System 的切换

主要应用到IK 去做攀爬时候的动作矫正：[好好玩游戏：Unity 头部IK 与手脚IK](http://www.xumenger.com/unity-ik-20210924/)

Github 上有对应实现该功能的代码：[https://github.com/conankzhang/procedural-climbing](https://github.com/conankzhang/procedural-climbing)
