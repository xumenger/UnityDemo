using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOTweenButtonClick : MonoBehaviour
{
    // 要求在Unity 编辑器中拖入对应Image 组件
    public RectTransform imageTransform;

    private bool isIN = false;

    private void Start()
    {
        // DOTween 提供了一下拓展方法，比如DOMove
        // 点击事件发生时，image 对象会在2s 内移动到Vector3(10, 10, 10)
        Tweener tweener = imageTransform.DOMove(new Vector3(10, 10, 10), 2);   // 修改世界坐标。动画默认播放完后会被销毁
                                                                               // Tweener 对象保存这个动画的信息，每次调用DO..() 类型方法时都会创建，这个对象由DOTween 来管理
                                                                               // 为了不每次点击时都创建一个Tweener 对象，所以在Start() 中创建一次，后续一直用
        tweener.SetAutoKill(false);  // 设置动画不自动销毁，DOPlayBackwards() 才可以倒放
        tweener.Pause();             // 不在这里执行动画，让后面在点击时执行

        tweener.OnComplete(OnTweenComplete);  // 设置动画播放完后回调的方法

        //imageTransform.DOLocalMove(new Vector3(10, 10, 10), 2); // 修改局部坐标
    }

    public void OnClick()
    {
        if (isIN == false)
        {
            // 移入
            imageTransform.DOPlayForward();   // 播放在Start() 中设置的动画
        }
        else
        {
            // 移出
            imageTransform.DOPlayBackwards();   // 倒放
        }

        isIN = !isIN;
    }

    void OnTweenComplete()
    {
        Debug.Log("动画播放完成");
    }
}
