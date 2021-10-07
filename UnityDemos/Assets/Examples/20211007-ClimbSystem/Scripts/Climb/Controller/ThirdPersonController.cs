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
        public float jumpSpeed = 5;

        bool onGround;
        bool keepOffGround;
        float savedTime;

        bool climbOff;
        float climbTimer;

        public bool isClimbing;

        FreeClimb freeClimb;


        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            col = GetComponent<Collider>();

            camHolder = CameraHolder.singleton.transform;

            anim = GetComponent<Animator>();

            freeClimb = GetComponent<FreeClimb>();
        }


        private void FixedUpdate()
        {
            if (isClimbing)
            {
                return;
            }

            onGround = OnGround();
            Movement();
        }


        void Movement()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            camYForward = camHolder.forward;
            Vector3 v = vertical * camHolder.forward;
            Vector3 h = horizontal * camHolder.right;

            moveDirection = (v + h).normalized;
            moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)));

            Vector3 targetDir = moveDirection;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * rotSpeed);
            transform.rotation = targetRot;

            Vector3 dir = transform.forward * (moveSpeed * moveAmount);
            dir.y = rigid.velocity.y;
            rigid.velocity = dir;
        }


        private void Update()
        {
            if (isClimbing)
            {
                freeClimb.Tick(Time.deltaTime);
                return;
            }

            onGround = OnGround();

            if (keepOffGround)
            {
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


        bool OnGround()
        {
            if (keepOffGround)
                return false;

            Vector3 origin = transform.position;
            origin.y += 0.4f;

            Vector3 direction = -transform.up;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, 0.41f))
                return true;

            return false;
        }

        public void DisableController()
        {
            rigid.isKinematic = true;
            col.enabled = false;
        }

        public void EnableController()
        {
            rigid.isKinematic = false;
            col.enabled = true;

            anim.CrossFade("jump up", 0.2f);

            climbOff = true;
            climbTimer = Time.realtimeSinceStartup;

            isClimbing = false;
        }

    }
}