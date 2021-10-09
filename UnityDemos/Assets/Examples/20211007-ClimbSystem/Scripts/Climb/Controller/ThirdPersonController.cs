using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    public class ThirdPersonController : MonoBehaviour
    {
        float horizontal;
        float vertical;

        Vector3 moveDirection;
        float moveAmount;

        Vector3 camYForward;
        Transform camHolder;

        Rigidbody rigid;
        Collider col;

        Animator anim;

        public float moveSpeed = 4;
        public float rotSpeed = 9;
        public float jumpSpeed = 15;

        bool onGround;
        bool keepOffGround;
        float savedTime;

        bool climbOff;
        float climbTimer;

        public bool isClimbing;   // 是否处于攀爬状态

        FreeClimb freeClimb;


        private void Start()
        {
            rigid = GetComponent<Rigidbody>();

            // Rigidbody中 Angular Drag  （角阻力）：同样指的是空气阻力，只不过是用来阻碍物体旋转的
            // 如果设置成无限的话，物体会立即停止旋转
            // 如果设置成0，物体在上升过程中，会发生侧翻旋转
            rigid.angularDrag = 999;

            // 设置刚体约束
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;


            col = GetComponent<Collider>();

            camHolder = CameraHolder.singleton.transform;

            anim = GetComponent<Animator>();

            freeClimb = GetComponent<FreeClimb>();
        }


        private void Update()
        {
            // 如果正处于攀爬状态
            if (isClimbing)
            {
                freeClimb.Tick(Time.deltaTime);
                return;
            }

            onGround = OnGround();

            // 如果脱离地面
            if (keepOffGround)
            {
                // realtimeSinceStartup 表示从游戏启动到当前的时间
                if (Time.realtimeSinceStartup - savedTime > 0.5f)
                {
                    keepOffGround = false;
                }
            }

            Jump();

            if (!onGround && !keepOffGround)
            {
                if (!climbOff)
                {
                    // 检查是否处于攀爬状态
                    isClimbing = freeClimb.CheckForClimb();
                    if (isClimbing)
                    {
                        DisableController();
                    }
                }
            }

            if (climbOff)
            {
                if (Time.realtimeSinceStartup - climbTimer > 1)
                {
                    climbOff = false;
                }
            }

            // 在Locomotion->jump up 的箭头上，调整两个动画的融合时间，可以调节动画过渡的效果
            // 这个也是调整动画参数的一个关键所在！
            anim.SetFloat("move", moveAmount);
            anim.SetBool("jump", !onGround);

        }


        // 根据输入判断是否跳跃
        void Jump()
        {
            if (onGround)
            {
                // 【Edit】=>【Project Settings】=>【Input Manager】
                bool jump = Input.GetButton("Jump");
                if (jump)
                {
                    Vector3 v = rigid.velocity;
                    v.y = jumpSpeed;
                    rigid.velocity = v;
                    savedTime = Time.realtimeSinceStartup;
                    keepOffGround = true;

                    //rigid.AddForce(transform.up * jumpSpeed);
                }
            }
        }


        private void FixedUpdate()
        {
            if (isClimbing)
            {
                return;
            }

            // 判断是否在地面上
            onGround = OnGround();

            // 角色移动
            Movement();
        }


        // TODO，后续这个可以优化为使用CharacterController 来实现角色移动
        void Movement()
        {
            // 使用传统Input API 获取水平、垂直的输入
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            // 获取相机前向
            camYForward = camHolder.forward;
            Vector3 v = vertical * camHolder.forward;
            Vector3 h = horizontal * camHolder.right;

            moveDirection = (v + h).normalized;
            // 限制value在0,1之间并返回value。如果value小于0，返回0。如果value大于1，返回1，否则返回value
            moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)));

            // 目标方向
            Vector3 targetDir = moveDirection;
            // 因为是在平面上移动，所以y（竖直）方向设置为0
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * rotSpeed);
            transform.rotation = targetRot;

            Vector3 dir = transform.forward * (moveSpeed * moveAmount);
            dir.y = rigid.velocity.y;
            rigid.velocity = dir;
        }


        // 判断角色是否处于地面上
        bool OnGround()
        {
            if (keepOffGround)
            {
                return false;
            }

            Vector3 origin = transform.position;
            origin.y += 0.4f;
            Vector3 direction = -transform.up;

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, 0.41f))
            {
                return true;
            }

            return false;
        }

        public void DisableController()
        {
            // isKinematic属性是确定刚体是否接受动力学模拟，此影响不仅包括重力感应，还包括速度、阻力、质量等的物理模拟
            rigid.isKinematic = true;

            col.enabled = false;
        }

        public void EnableController()
        {
            rigid.isKinematic = false;
            col.enabled = true;

            /**
             * CrossFade是按照动画的自身时间进行混合。如果动画10秒，混合持续时间0.2，会在2秒后混合完成
             * CrossFadeInFixedTime是按照实际时间进行混合。如果动画10秒，混合持续时间0.2，会在0.2秒后混合完成
             * 使用CrossFade或CrossFadeInFixedTime混合时，如果混合时间大于动画自身长度，动画自身会被滞留在最后一帧，直到混合结束
             * 
             * 使用这个方法可以不用再在Animator 上连各种状态转移的箭头了
             * 可以通过代码逻辑控制不同状态的切换！
             * 而且支持动画的融合效果！
             * 
             * 使用该方法，然后自己在代码中通过数据结构管理状态之间的切换实现不同的动作效果
             * 这个是一个不错的思路！
             */
            anim.CrossFade("jump up", 0.2f);

            climbOff = true;
            climbTimer = Time.realtimeSinceStartup;

            //isClimbing = false;
        }

    }
}