using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace example.y20210925
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementInput : MonoBehaviour
    {
        private Animator anim;
        private Camera cam;
        private CharacterController controller;

        private Vector3 desiredMoveDirection;
        private Vector3 moveVector;

        public Vector2 moveAxis;   // 通过Input System 获取该值
        private float verticalVel; // 

        [Header("Settings")]
        [SerializeField] float movementSpeed;          // 移动速度
        [SerializeField] float rotationSpeed = 0.1f;   // 旋转速度
        [SerializeField] float fallSpeed = 0.2f;       // 下落速度
        public float acceleration = 1;                 // 加速度

        [Header("Booleans")]
        [SerializeField] bool blockRotationPlayer;
        private bool isGrounded;

        private void Start()
        {
            anim = this.GetComponent<Animator>();
            cam = Camera.main;
            controller = this.GetComponent<CharacterController>();
        }

        private void Update()
        {
            // 处理玩家输入
            InputMagnitude();

            isGrounded = controller.isGrounded;

            // 判断玩家是否在地面上，确定下落速度
            if (isGrounded)
                verticalVel -= 0;
            else
                verticalVel -= 1;

            // CharacterController 控制移动
            moveVector = new Vector3(0, verticalVel * fallSpeed * Time.deltaTime, 0);
            controller.Move(moveVector);
        }

        // 玩家移动与旋转
        // 现在WASD 控制玩家移动的效果很差，还没有定位出来原因！
        void PlayerMoveAndRotation()
        {
            // 获取相机的朝向
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            // 计算预期移动的方向
            desiredMoveDirection = forward * moveAxis.y + right * moveAxis.x;

            if (blockRotationPlayer == false)
            {
                // Camera
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * acceleration);
                controller.Move(desiredMoveDirection * Time.deltaTime * (movementSpeed * acceleration));
            }
            else
            {
                // Strafe
                controller.Move((transform.forward * moveAxis.y + transform.right * moveAxis.y) * Time.deltaTime * (movementSpeed * acceleration));
            }
        }

        // 旋转看向pos的位置
        public void LookAt(Vector3 pos)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), rotationSpeed);
        }

        // 向摄像机旋转
        public void RotateToCamera(Transform t)
        {
            var forward = cam.transform.forward;

            desiredMoveDirection = forward;
            Quaternion lookAtRotation = Quaternion.LookRotation(desiredMoveDirection);
            // Quaternion.Euler() 方法的作用是什么
            Quaternion lookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, lookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            t.rotation = Quaternion.Slerp(transform.rotation, lookAtRotationOnly_Y, rotationSpeed);
        }

        // 处理输入
        void InputMagnitude()
        {
            /**
             * Calcurate the Input Magnitude
             * Vector3.magnitude 是指向量的长度
             * Vector3.sqrMagnitude 是指向量长度的平方
             * 在Unity当中使用平方的计算要比计算开方的速度快很多
             * 因为向量的大小由勾股定理得出，所以有开方操作，所以如果只是单纯比较向量之间的大小的话，建议使用Vector3.sqrMagnitude进行比较即可。提高效率和节约性能
             */
            float inputMagnitude = new Vector2(moveAxis.x, moveAxis.y).sqrMagnitude;

            // Physically move player
            if (inputMagnitude > 0.1f)
            {
                anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, 0.1f, Time.deltaTime);
                PlayerMoveAndRotation();
            }
            else
            {
                anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, 0.1f, Time.deltaTime);
            }

        }

        #region Input

        // Input System 的用法继续研究？
        public void OnMove(InputValue value)
        {
            moveAxis.x = value.Get<Vector2>().x;
            moveAxis.y = value.Get<Vector2>().y;
        }

        #endregion

        // OnDisable() 什么时候会被回调？
        private void OnDisable()
        {
            anim.SetFloat("InputMagnitude", 0);
        }
    }
}