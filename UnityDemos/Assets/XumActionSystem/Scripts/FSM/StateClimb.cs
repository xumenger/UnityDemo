using UnityEngine;

namespace xum.action
{
    /// <summary>
    /// * 在角色身上创建一个ClimbHelper 游戏物体（Cube），用于后续检测与墙的接触，并且拖到Climb 脚本的climbHelper 上
    ///   用于确保玩家与墙的距离，这样玩家不会直接撞到墙上
    ///   怎么实现不显式在角色上添加这个ClimbHelper 物体，而是通过代码在启动时生成？
    /// * 攀爬需要的动作从mixamo 下载，存放在Climb Animation 文件夹下
    /// * 新建一个Wall 层，为场景中的墙设置Wall 层
    ///   并且为Climb 脚本的EnvLayer 参数选择Wall 层
    /// * 动画状态机、动画设置
    ///   EnterClimb -> Climbing Blend Tree 的Has Exit Time 取消勾选
    ///   Braced Hang Shimmy 设置 Root Transform Rotation: Original；Root Transform Position(Y): Feet；Root Transform Position(XZ): Original
    ///   Hanging Idle (1) 进行同样的设置
    ///   Climbing Up Wall 进行同样的设置
    ///   Climbing Down Wall 进行同样的设置
    /// * 以上四个动作注意都要勾选Loop Time，否则播放一次后就停止了！
    ///   以上四个动作注意必须还要勾选Loop Pose，否则即使取消勾选Animator 的Apply Root Motion 之后，动画的位移还是会产生影响！
    ///   其他的动作也是一样，如果希望在取消勾选Animator 的Apply Root Motion 之后能够不让动作的位移产生影响，则动画的Loop Pose 必须勾选！
    /// * 注意在Climb Blend Tree 中对于Braced Hang Shimmy 镜像、速度的设置
    /// * 因为PlayerController 现在也有对于Input 输入的处理，测试的时候暂时将PlayerController 关闭
    ///   等待后续将攀爬动作重构到状态机系统中
    /// * 判断到达墙边触发爬墙动作这个应该用事件模型进行重构
    ///
    ///
    ///
    /// 与攀爬动作相关的事件和事件处理类是
    /// EventPlayerStartClimb
    /// ActionPlayerStartClimb
    /// 
    /// </summary>
    public class StateClimb : FSMState
    {
        // 是否在墙上
        public bool onWall;

        // 身体的目标位置，如果从离墙比较远的位置要到墙上的话，不能直接贴着墙，需要有一个目标位置，用这个变量保存
        private Vector3 targetPos;

        // 射线长度
        public float wallRayLength = 1;

        // 离墙的距离
        public float wallOffset = 0.5f;

        private CharacterController controller;

        public Transform climbHelper;
        private Vector3 headPos;
        private RaycastHit hitInfo;

        // 攀爬速度
        public float climbSpeed = 0.5f;

        public StateClimb(GameObject gameObject,
                          Animator animator,
                          CharacterController controller,
                          FSMManager fsmManager) : base(EChangeType.eTrigger, gameObject, animator, fsmManager)
        {
            this.controller = controller;
        }


        /// <summary>
        /// 这里的射线检测逻辑和EventPlayerStartClimb 中的重复了
        /// 但是这里需要拿到射线信息
        /// 所以暂时用这种重复编码的方式实现
        /// 
        /// </summary>
        public override void OnStart()
        {
            Vector3 origin = gameObject.transform.position;
            Vector3 dir = gameObject.transform.forward;

            // 使用射线检测是否走到墙边，hit 表示射线命中墙的位置
            RaycastHit hit;

            if (Physics.Raycast(origin, dir, out hit, wallRayLength))
            {
                onWall = false;
                // targetPos 的位置等于玩家根据射线检测后应该移动到的位置
                targetPos = hit.point + hit.normal * wallOffset;

                // 播放到墙上的动画
                animator.CrossFade("EnterClimb", 0.2f);
            }

            
        }


        public Vector2 input;
        public override void OnUpdate()
        {

            // 爬墙的移动通过BlendTree 实现，这里通过设置攀爬动作的参数调整动画效果
            // 读取键盘输入，去播放动画
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            animator.SetFloat(AnimatorEnum.Anim_F_ClimbHorizontal, input.x);   // 水平左右移动
            animator.SetFloat(AnimatorEnum.Anim_F_ClimbVertical, input.y);     // 垂直上下移动

            // EnterClimb -> Climbing Blend Tree 的Has Exit Time 取消勾选
            // 两个动画之间的切换必须要一个条件，在Animator 中通过Base Layer.climb 这个触发器触发动作切换
            // 根据是否有移动的输入判断是否切换
            if (input.magnitude > 0)
            {
                animator.SetTrigger(AnimatorEnum.Anim_T_Climb);
            }

            if (!onWall)
            {
                // 如果不在墙上，则检查是否可以进入攀爬状态
                SetBodyPositionToWall();
            }
            else
            {
                // 如果正在爬墙，则调整角色位置。目前注释掉这一行的话会正常播放动作，为什么？
                //FixBodyPos();

                // 处理运动
                MoveHandle();
            }
        }



        /// <summary>
        /// 处理IK
        /// </summary>
        private Vector3 LeftHandIK, RightHandIK, LeftFootIK, RightFootIK;
        private Vector3 LeftHandPos, RightHandPos, LeftFootPos, RightFootPos;
        private Quaternion LeftHandRot, RightHandRot, LeftFootRot, RightFootRot;
        public bool enableIK;

        // 脚步IK 位置与实际射线检测位置的Y轴差
        [Range(0, 0.2f)] public float GroundOffset;

        // 射线向下检测距离
        public float GroundDistance;

        // IK 交互层
        public LayerMask EnvLayer;
        public float climbRotateSpeed = 5f;
        public float offset = 0.2f;

        public override void OnFixedUpdate()
        {
            Debug.DrawLine(LeftHandIK - gameObject.transform.forward, LeftHandIK + gameObject.transform.forward * GroundDistance, Color.blue, Time.fixedDeltaTime);
            Debug.DrawLine(RightHandIK - gameObject.transform.forward, RightHandIK + gameObject.transform.forward * GroundDistance, Color.blue, Time.fixedDeltaTime);

            // 左手IK 设置
            if (Physics.Raycast(LeftHandIK - gameObject.transform.forward, gameObject.transform.forward, out RaycastHit hit, GroundDistance + 1, EnvLayer))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, Time.fixedDeltaTime);

                LeftHandPos = hit.point - gameObject.transform.forward * GroundOffset;
                LeftHandRot = Quaternion.FromToRotation(-gameObject.transform.forward, hit.normal) * gameObject.transform.rotation;
            }

            // 右手IK 设置
            if (Physics.Raycast(RightHandIK - gameObject.transform.forward, gameObject.transform.forward, out RaycastHit hit1, GroundDistance + 1, EnvLayer))
            {
                Debug.DrawRay(hit1.point, hit1.normal, Color.red, Time.fixedDeltaTime);

                RightHandPos = hit1.point - gameObject.transform.forward * GroundOffset;
                RightHandRot = Quaternion.FromToRotation(-gameObject.transform.forward, hit1.normal) * gameObject.transform.rotation;
            }

            // 在上下爬的时候会遇到拐角，需要调整旋转，否则视觉效果会很差
            // 先用射线检测：在头顶向前检测
            // hit2 是从我们的身体前方发射射线，返回它的法线
            if (Physics.Raycast(gameObject.transform.position + gameObject.transform.up * offset, gameObject.transform.forward, out RaycastHit hit2, 0.5f))
            {
                // 因为爬墙的时候，手部会先接触到拐角，拐角的法线就不再是垂直的，与身体的法线会形成一个夹角
                // 当这个夹角大于1度，y<0 表示向下爬，就去旋转
                if (Vector3.Angle(hit2.normal, hit.normal) > 1f && input.y < 0)
                {
                    gameObject.transform.Rotate(gameObject.transform.right, climbRotateSpeed * Time.deltaTime);
                }

                // y>0 表示向上爬，身体后方与手部法线有夹角的话，对应旋转
                if (Vector3.Angle(-gameObject.transform.forward, hit.normal) > 1f && input.y > 0)
                {
                    gameObject.transform.Rotate(gameObject.transform.right, -climbRotateSpeed * Time.deltaTime);
                }
            }
        }


        public override void OnAnimatorIK(int layerIndex)
        {
            LeftHandIK = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            RightHandIK = animator.GetIKPosition(AvatarIKGoal.RightHand);

            if (enableIK == false)
            {
                return;
            }

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPos);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandRot);

            animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandPos);
            animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandRot);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetBodyPositionToWall()
        {
            // 如果当前玩家位置与射线命中墙的位置相差0.01f，则进入爬墙状态
            if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.01f)
            {
                onWall = true;
                gameObject.transform.position = targetPos;
                return;
            }

            // 通过MoveTowards 控制玩家位置向射线命中的目标位置移动
            Vector3 lerpTargetPos = Vector3.MoveTowards(gameObject.transform.position, targetPos, 0.2f);
            gameObject.transform.position = lerpTargetPos;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FixBodyPos()
        {
            // InverseTransformPoint 变换位置从世界坐标到局部坐标。和Transform.TransformPoint相反。
            Vector3 localClimbHelperPos = gameObject.transform.InverseTransformPoint(climbHelper.position);

            Vector3 localHeadPos = new Vector3(0, localClimbHelperPos.y, 0);
            headPos = gameObject.transform.TransformPoint(localHeadPos);

            Debug.DrawRay(headPos, gameObject.transform.forward * 1f, Color.red);
            if (Physics.SphereCast(headPos, 0.1f, gameObject.transform.forward, out hitInfo, 1))
            {
                Vector3 tempVector = gameObject.transform.position - climbHelper.position;
                if (Vector3.Distance(gameObject.transform.position, hitInfo.point + tempVector) > 0.05f)
                {
                    gameObject.transform.position = hitInfo.point + tempVector;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void MoveHandle()
        {
            bool canMove = true;
            if (input.magnitude > 0.05f && canMove)
            {
                // Transform.Translate通过设置下一步移动的矢量方向和大小进行移动
                //Debug.Log()
                gameObject.transform.Translate(input.x * climbSpeed * Time.deltaTime, input.y * climbSpeed * Time.deltaTime, 0);

            }
            else
            {
                // 播放动画
                animator.SetFloat(AnimatorEnum.Anim_F_ClimbHorizontal, 0);
                animator.SetFloat(AnimatorEnum.Anim_F_ClimbVertical, 0);
            }
        }
    }

}