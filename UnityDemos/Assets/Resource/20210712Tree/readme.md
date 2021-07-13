将【Models】下面的fengshu\_01 模型拖入场景中

【Window】=>【Rendering】=>【Lighting Settings】，勾选Auto Generate

【Window】=>【Package Manager】，搜索并安装“Universal RP”

>参考[Unity 可编程渲染管线](http://www.xumenger.com/unity-render-pipeline-20201207/)

>参考[Unity 2018之ShaderGraph使用基础](https://blog.csdn.net/u011618339/article/details/80319955)

选中Materials 下面的leaf 1、leaf、thunk 三个材质，【Edit】=>【Render Pipeline】=>【Universal Render Pipeline】=>【Upgrade Selected Materials to UniversalRP Materials】

这个时候SingleVegetation.shadergraph，以及用到这个ShaderGraph 的各个材质都是不正常的

在【Assets】根目录下，【右键】=>【Create】=>【Universal Render Pipeline】=>【Pipeline Asset（Forward Renderer）】

【Edit】=>【Project Settings】=>【Graphics】，选择刚才创建的资源

【Edit】=>【Project Settings】=>【Graphics】在Scriptable Render Pipeline Settings 选择刚才创建的URP 配置资源

【Edit】=>【Render Pipeline】=>【Universal Render Pipeline】=>【Upgrade Project Materials to UniversalRP Materials】

涉及到的材质

* leaf 1、leaf：树叶
* thunk：树干

主要通过Shader、各种贴图做出来树木的效果，当然最基础的就是树木的模型！