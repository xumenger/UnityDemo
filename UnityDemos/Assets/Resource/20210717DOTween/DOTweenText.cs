using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DOTweenText : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        text.DOText("接下来进入第二关，开启新的征程", 2);   // 默认是一个字一个字显示的动画效果
        // DOText() 还有更多参数，比如用来设置文本颜色等，对应参考官方文档

        // text.DOColor(Color.blue, 2); 2秒钟内文本变成蓝色
        // text.DOFade(1, 3);   // 3秒内文本由没有渐变显示出来，实际是修改Color 的Alpha 值
    }
}
