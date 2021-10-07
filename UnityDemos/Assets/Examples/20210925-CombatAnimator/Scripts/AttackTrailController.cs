using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20210925
{
    public class AttackTrailController : MonoBehaviour
    {
        // TrailRenderer 类的作用是什么？
        // LineRender和TrailRender是两个好东西，很多Unity拖尾特效都会使用到它们
        // https://www.cnblogs.com/driftingclouds/p/6442847.html
        // 如何在动作上添加挥拳、踢腿的扫尾效果？
        [SerializeField] private TrailRenderer trail;

        [Header("Reference")]
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightFoot;
        [SerializeField] private Transform leftFoot;

        public void SetTrailParent(int type)
        {
            switch (type)
            {
                case 0:
                    trail.transform.parent = rightHand;
                    break;
                case 1:
                    trail.transform.parent = leftHand;
                    break;
                case 2:
                    trail.transform.parent = rightFoot;
                    break;
                case 3:
                    trail.transform.parent = rightFoot;
                    break;
                default:
                    trail.transform.parent = leftFoot;
                    break;
            }
        }
    }
}