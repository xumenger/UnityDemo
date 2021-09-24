using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20210924
{
    public class HandIK : MonoBehaviour
    {
        public Animator animator;

        public GameObject positionGoal;
        public GameObject rotationGoal;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        // 说明，在HeadIK 中处理goalPosition 的移动，和HeadIK 的lookAt 使用一个GameObject

        void OnAnimatorIK(int layerIndex)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, positionGoal.transform.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            animator.SetIKRotation(AvatarIKGoal.LeftHand, rotationGoal.transform.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        }
    }
}