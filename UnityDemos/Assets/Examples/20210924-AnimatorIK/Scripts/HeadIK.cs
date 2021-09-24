using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20210924
{
    public class HeadIK : MonoBehaviour
    {
        public Animator animator;

        public GameObject lookAt;
        public bool moveToLeft = true;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void OnAnimatorIK(int layerIndex)
        {
            // 这个方法用来设置头部看向的位置，比如看向一个Cube，头就会相应的旋转
            animator.SetLookAtPosition(lookAt.transform.position);

            /**
             * public void SetLookAtWeight(float weight, float bodyWeight = 0.0f, float headWeight = 1.0f, float eyesWeight = 0.0f, float clampWeight = 0.5f);
             * 这个方法用来设置IK的权重，这个IK会和原来的动画进行混合。如果权重为1，则完全用IK的位置旋转；如果权重为0，则完全用原来动画中的位置和旋转。至少要设置第一个参数，后面的几个参数都有默认值
             * 
             * Weight 全局权重，后面所有参数的系数
             * bodyWeight 身体权重，身体参与LookAt的程度，一般是0
             * headWeight 头部权重，头部参与LookAt的权重，一般是1
             * eyesWeight 眼睛权重，眼睛参与LookAt的权重，一般是0（一般没有眼睛部分的骨骼）
             * clampWeight 权重的限制。0代表没有限制（脖子可能看起来和断了一样），1代表完全限制（头几乎不会动，像是固定住了）。0.5代表可能范围的一半（180度）
             */
            animator.SetLookAtWeight(1);
        }


        // 在Update 中移动Cube，测试角色是否一直看向Cube
        private void Update()
        {
            Vector3 begin = lookAt.transform.position;

            Vector3 end;

            if (lookAt.transform.position.x > 1 && moveToLeft)
            {
                moveToLeft = false;
            }
            else if (lookAt.transform.position.x < -1 && !moveToLeft)
            {
                moveToLeft = true;
            }

            if (moveToLeft)
            {
                end = new Vector3(1.1f, 1, 1);
            }
            else
            {
                end = new Vector3(-1.1f, 1, 1);
            }

            Vector3 target = Vector3.Lerp(begin, end, Time.deltaTime);

            lookAt.transform.position = target;
        }

    }


}