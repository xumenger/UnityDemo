using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CubePositionChange : MonoBehaviour
{
    private Vector3 myValue = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        // 指定在2s 时间内，myValue 的值变化到Vector(10, 10, 10)
        // 第一个参数是() => myValue，Lambda 表达式，返回myValue
        // 第二个参数是x => myValue = x，也是Lambda 表达式，x 与目标值new Vector(10, 10, 10) 计算一个插值，赋值给myValue
        DOTween.To(() => myValue, x => myValue = x, new Vector3(10, 10, 10), 2);
    }

    // Update is called once per frame
    void Update()
    {
        // 在Update 中用myValue 的值对应更新Cube 的Position
        transform.position = myValue;
    }
}
