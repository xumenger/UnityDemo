using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOTweenShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 实现随机震动，第一个参数是震动时间；第二个参数是震动强度，这里指定仅在X、Y轴震动
        transform.DOShakePosition(1, new Vector3(1, 1, 0));
    }
}
