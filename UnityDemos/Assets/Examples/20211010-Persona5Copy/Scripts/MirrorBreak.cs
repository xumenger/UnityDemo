using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace example.y20211010
{
    
    public class MirrorBreak : MonoBehaviour
    {
        // 这些值在Unity 中手动拖入
        public float breakDuration;
        public Transform cam;
        public Transform mirrorParent;

        // Start is called before the first frame update
        void Start()
        {
            // 遍历所有mirror 中的所有“碎片”
            for (int i = 0; i < mirrorParent.childCount; i++)
            {
                // 随机旋转
                mirrorParent.GetChild(i).DOLocalRotate(new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20)), breakDuration);
                // 随机缩放
                mirrorParent.GetChild(i).DOScale(mirrorParent.GetChild(i).localScale / 1.1f, breakDuration);

                // 屏幕震动
                cam.DOShakePosition(breakDuration, 0.5f, 20, 90, false, true);
            }
        }
    }

}