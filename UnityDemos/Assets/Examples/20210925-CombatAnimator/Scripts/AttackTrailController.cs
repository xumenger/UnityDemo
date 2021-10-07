using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20210925
{
    public class AttackTrailController : MonoBehaviour
    {
        // TrailRenderer 类的作用是什么？
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