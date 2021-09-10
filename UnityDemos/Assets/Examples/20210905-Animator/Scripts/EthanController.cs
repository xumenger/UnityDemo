using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    public float speed = 2.0F;
    public float jumpSpeed = 6.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    bool isWalk = false;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // https://blog.csdn.net/H2ojunjun/article/details/104829330
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            // 根据移动速度控制播放不同的动画
            if (moveDirection.magnitude - 0f > 0.0001f)
            {
                if (!isWalk)
                {
                    animator.SetTrigger("Walk");
                    isWalk = true;
                }  
            }
            else
            {
                animator.SetTrigger("Idle");
                isWalk = false;
            }

            // 空格键实现跳跃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // 受“重力”控制向下
        moveDirection.y -= gravity * Time.deltaTime;

        // 角色控制器实现角色移动
        controller.Move(moveDirection * Time.deltaTime);
    }
}
