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

## ShaderGraph 报错问题

但是从别的项目拷贝过来的SingleVegetation.shadergraph 还是有问题，在项目里无法被识别，最后发现是因为当前项目使用的是Unity.2019.4.15 版本，而另一个项目是Unity.2020.3.1f1c1，将当前项目升级为Unity.2020.3.1f1c1 就好了！

但是这样只是可以正确的识别这个 ShaderGraph，实际还是无法运行，打开这个ShaderGraph，会有这样的报错

```
Validation: Could not find Sub Graph asset
with GUID
fff11475ffc1e6f409eda9cbbca81bf.
```

或者在Console 上会看到类似这样的报错

```
Error in Graph at Assets/Resource/20210712Tree/Shaders/SmoothTriangleWave.shadersubgraph at node SmoothCurve: Validation: Could not find Sub Graph asset with GUID 46eb8e1b7f0738a46bfcae7099ba1507.
UnityEditor.AssetImporters.ScriptedImporter:GenerateAssetData (UnityEditor.AssetImporters.AssetImportContext) (at /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ScriptedImporter.cs:22)
```

找到原来项目中的SingleVegetation.shadergraph ，双击打开后，在对应的ShaderGraph 上右键，【Open Sub Graph】，找到对应的依赖的Sub Graph，然后拷贝到这个项目里

当然可能存在层层依赖，就按照这样的顺序，找到所有的依赖

最终拷贝过来的ShaderGraph 包括

* SingleVegetation.shadergraph
* SmoothCurve.shadersubgraph
* SmoothTriangleWave.shadersubgraph
* VegetationDeformation.shadersubgraph
* TriangleWave.shadersubgraph

但是直接拷贝放到一个目录下还是会报错，按照原来项目的**相对路径**拷贝过来可以解决问题！

## 材质梳理

涉及到的材质：

* leaf 1、leaf：树叶
* thunk：树干

为树木增加这三个材质，然后对应材质球上的VegetationTexture、VegetationNormal 选择对应的材质贴图和法线贴图，其他参数参考原来的项目做对应的设置！

主要通过Shader、各种贴图做出来树木的效果，当然最基础的就是树木的模型！

>材质、贴图能够和树木模型的树干/树叶一一对应上，应该是UV 的作用吧？

>研究这几个ShaderGraph 的实现原理！

>如果打开Lighting Setting 的【Auto Generate】的话，每次对场景的轻微改动，都会导致重新渲染光照贴图，你会观察到风扇转的厉害！！

