实现奎爷飞斧的动作技能

>[【Unity教程 附源码】复刻《战神4》奎爷飞斧 技能效果 U3D教程](https://www.bilibili.com/video/BV1gJ411p7FM)

## 美术资源

参考这个项目，在mixamo 找到相应的动作，只找到了idle、walking 动作，其他的aim、pull、pull2、throw 

将模型、动作的Animation Type 都设置为Generic，注意不要设置为Humanoid

将参考项目中的所有模型，包括可以破碎的盒子（Box）、斧头（LEVIATHAN Axe_lowAO） 拿到我的项目中直接使用！

## 预制体修复

上面的方式将模型、预制体、材质直接拷贝过来的方式，可能因为目录变更等问题导致出现问题，比如材质丢失、预制体的组件丢失问题，需要修复

将参考项目中的所有材质拿到我的项目中直接使用！选中所有的材质将其更新为URP 材质！

选中预制体，点击Inspector 上的Open Prefab，然后进行修复

* BoxBroken Variant 预制体
	* 添加Rigidbody 组件
	* 添加BoxCollider 组件
	* 添加BreakBoxScript.cs 脚本，这个先从参考项目中拷贝过来，解决这个问题，后续自己再抄一遍
* BoxUnbroken Variant 预制体
	* 添加Rigidbody 组件
	* 添加BoxCollider 组件
	* 添加BreakBoxScript.cs 脚本，这个先从参考项目中拷贝过来，解决这个问题，后续自己再抄一遍
		* Breaked Box 属性设置上面的BoxBroken Variant 预制体


## ybot设置

>为其添加Player Layer

将ybot 模型拖到场景中，在Hierarchy 中展开ybot 游戏对象，将LEVIATHAN Axe_lowAO 放到mixamorig:RightHand 上

>有没有什么更好的动态拿武器、更换武器的方式，代码实现！

在ybot 上添加空物体CurvePoint，将其作为曲线中间点，在实现斧头按照曲线飞回来的功能上需要使用，CurvePoint 相对于ybot 的位置为(2.769, 1.31, 1.59)，这个点位于角色的右前侧，其位置会相对ybot 移动也进行变化，其作用是在斧头飞回来的时候一定会经过这个点，具体可以看代码实现

为ybot 添加Animator 组件，设置对应的动画状态机

为ybot 添加Character Controller 组件

为ybot 添加ThrowController.cs 脚本，并对照参考项目设置对应的参数

* Weapon
* Hand
* Spine
* Curve Point
* 粒子特效，等后面再设置粒子特效
	* Glow Particle
	* Catch Particle
	* Trail Particle
	* Trail Renderer
* UI
* 虚拟相机，等后续对应创建和设置虚拟相机，创建Free Look
	* Virtual Camera
	* Impulse Source

为ybot 添加MovementInput.cs 脚本，并对照参考项目设置对应的参数

* Anim

## 虚拟相机

创建Free Look Camera，将Follow、Look At 设置为ybot，使其跟随玩家

为CM FreeLook1 添加Cinemachine Impulse Source 脚本

## 斧头游戏对象设置

>为其添加Weapon Layer

设置大小、位置，使其刚好放在模型的手上

添加Rigidbody 组件，勾选Is Kinematic

添加Box Collider 组件

添加WeaponScript.cs 脚本

为斧头设置如下材质（也是从参考项目中拷贝过来的材质）

* grid
* rope
* wood
* metal

## 动画状态机设置/动画事件

对照参考项目的动画状态机，设置动画状态转移、变量……

在Throw 动作上，对应扔出斧头的动作处设置一个动画事件，命名为WeaponThrow

Idle、Pull、 的动画的Loop Time 注意勾选

## 场景搭建

>创建一个Level 游戏物体，设置Layer 为Ground

简单的创建一个围墙，用于后续斧头砍到上面，注意为所有的围墙增加一个Mesh Collider，否则无法实现斧头扔到墙上就钉在墙上的效果

在场景中放置一些可破裂的盒子，用于测试飞斧的攻击效果！

## Debug 经验

在复刻的过程中出现一个问题，就是在按下鼠标右键后，一直无法切换到瞄准状态

最后检查发现实在Animator Controler 中，Idle 切换到Aim 的Bool 变量我错误的命名为animing，实际应该是aiming！！！

在移动的时候，因为从miamo 上下载的fbx 动画不能设置为Humanoid，只能设置为Generic，无法设置Root Transform Position（XZ），所以就会出现动画的位移也会导致角色的位移，而我只是想用这个动画播放动作，位移希望由Character Controller 来实现，所以暂时的解决方法是使用参考项目中的Walking 动作！

还有一个斧头扔到墙上无法钉在墙上的问题，因为在OnCollisionEnter() 对于Ground 这个Layer 的判断错误，应该改成我的项目中的12

```c#
// 武器上加了一个Box Collider，当和其他物体碰撞的时候，触发该函数
private void OnCollisionEnter(Collision collision)
{
    // 原来的代码中，这部分是11，因为原来项目中Ground 是11 层，但是我的复刻项目中Ground 是12 层注意修改
    if (collision.gameObject.layer == 12)
    {
        print(collision.gameObject.name);
        GetComponent<Rigidbody>().Sleep();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        GetComponent<Rigidbody>().isKinematic = true;
        activated = false;
    }

}
```

这些解决具体问题的经验可以加深我对于Unity 的动画、物理系统的理解！！！

## 代码实现

对应看代码中的备注！！！

## 遗留内容

对应的粒子效果、屏幕后处理效果，没有在这次复刻中对应实现！

而且现在的操作感觉还不是很舒服，这个可能需要我再去慢慢的调整输入和动画的匹配度！

这一遍只是参考项目，通过配置的方式将效果实现，代码只是认真地看了一遍并且注释，代码也没有自己动手跟这写

其中对于如何实现在移动鼠标的时候，镜头对应跟着旋转的实现要好好研究

另外思考，这个攻击技能的实现，如何封装到动作模块中？！？！？！
