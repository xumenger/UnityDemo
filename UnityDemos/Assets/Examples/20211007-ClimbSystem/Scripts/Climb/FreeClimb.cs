using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace example.y20211007
{
    public class FreeClimb : MonoBehaviour
    {
        public Animator anim;

        public bool isClimbing;
        public bool isMid;

        bool inPosition;
        bool isLerping;

        float t;
        float delta;

        //float posT;
        Vector3 startPos;
        Vector3 targetPos;
        //Quaternion startRot;
        //Quaternion targetRot;

        public float positionOffset = 1.0f;
        public float offsetFromWall = 0.3f;
        public float speed_mutiplier = 0.2f;
        public float climbSpeed = 3;
        public float rotateSpeed = 5;

        public float distanceToWall = 1;
        public float distanceToMoveDirection = 1.0f;

        //public float rayTowardsMoveDir = 0.5f;
        //public float rayForwardTowardsWall = 1;

        //public float horizontal;
        //public float vertical;

        // IKSnapshot 是在本源文件中自定义的类
        public IKSnapshot baseIKsnapshot;

        public FreeClimbAnimHook a_hook;    // 在Unity编辑器中手动拖入

        Transform helper;

        // 第三人称控制器
        ThirdPersonController tpc;

        LayerMask ignoreLayers;


        // Start is called before the first frame update
        void Start()
        {
            tpc = GetComponent<ThirdPersonController>();

            Init();
        }

        public void Tick()
        {
            delta = Time.deltaTime;
            Tick(delta);
        }

        private void Init()
        {
            anim = GetComponent<Animator>();

            helper = new GameObject().transform;
            helper.name = "climb helper";
            a_hook.Init(this, helper);

            // Controller 这个层排在第11
            ignoreLayers = ~(1 << 11);
        }


        // 在ThirdPersonController 脚本中的Update() 方法中会调用这个Tick() 方法
        public void Tick(float deltaTime)
        {
            this.delta = deltaTime;

            if (!inPosition)
            {
                GetInPosition();
                return;
            }

            if (!isLerping)
            {
                // 按X 后，取消爬墙，从墙上跳下来
                bool cancel = Input.GetKeyUp(KeyCode.X);
                if (cancel)
                {
                    CancelClimb();
                    return;
                }

                float hor = Input.GetAxis("Horizontal");
                float vert = Input.GetAxis("Vertical");
                float m = Mathf.Abs(hor) + Mathf.Abs(vert);

                Vector3 h = helper.right * hor;
                Vector3 v = helper.up * vert;
                Vector3 moveDir = (h + v).normalized;

                if (isMid)
                {
                    if (moveDir == Vector3.zero)
                    {
                        return;
                    }
                }
                else
                {
                    bool canMove = CanMove(moveDir);
                    if (!canMove || moveDir == Vector3.zero)
                    {
                        return;
                    }
                }

                isMid = !isMid;

                t = 0;
                isLerping = true;
                startPos = transform.position;
                Vector3 tp = helper.position - transform.position;
                float d = Vector3.Distance(helper.position, startPos) / 2;
                tp *= positionOffset;
                tp += transform.position;
                targetPos = (isMid) ? tp : helper.position;

                // enable the ik
                a_hook.CreatePositions(targetPos, moveDir, isMid);
            }
            else
            {
                t += delta * climbSpeed;
                if (t > 1)
                {
                    t = 1;
                    isLerping = false;
                }

                Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
                transform.position = cp;
                transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);

                LookForGround();
            }
        }

        public bool CheckForClimb()
        {
            // 获取角色的位置
            Vector3 origin = transform.position;
            origin.y += 0.02f;
            Vector3 dir = transform.forward;

            // 通过射线判断是否射线击中“墙”
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, 0.5f, ignoreLayers))
            {
                helper.transform.position = PosWithOffset(origin, hit.point);
                InitForClimb(hit);

                return true;
            }

            return false;
        }

        void InitForClimb(RaycastHit hit)
        {
            isClimbing = true;

            a_hook.enabled = true;

            helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
            startPos = transform.position;
            targetPos = hit.point + (hit.normal * offsetFromWall);
            t = 0;
            inPosition = false;

            // CrossFade是按照动画的自身时间进行混合。如果动画10秒，混合持续时间0.2，会在2秒后混合完成
            // 可以实现当前动画融合过渡到指定的动画动作
            anim.CrossFade("hanging idle", 2);
        }


        bool CanMove(Vector3 moveDir)
        {
            Vector3 origin = transform.position;
            float dis = distanceToMoveDirection;
            Vector3 dir = moveDir;

            DebugLine.singleton.SetLine(origin, origin + (dir * dis), 0);

            // Raycast towards the direction you want to move
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, dis))
            {
                // Check if it's a corner
                return false;
            }

            origin += moveDir * dis;
            dir = helper.forward;

            float dis2 = distanceToWall;
            DebugLine.singleton.SetLine(origin, origin + (dir * dis2), 1);

            // Raycast forward towards the wall
            if (Physics.Raycast(origin, dir, out hit, dis))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }

            origin = origin + (dir * dis2);
            dir = -moveDir;

            DebugLine.singleton.SetLine(origin, origin + dir, 1);

            // Raycast for inside corners
            if (Physics.Raycast(origin, dir, out hit, distanceToWall))
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }

            origin += dir * dis2;
            dir = -Vector3.up;

            DebugLine.singleton.SetLine(origin, origin + dir, 2);

            if (Physics.Raycast(origin, dir, out hit, dis2))
            {
                float angle = Vector3.Angle(-helper.forward, hit.normal);
                if (angle < 40)
                {
                    helper.position = PosWithOffset(origin, hit.point);
                    helper.rotation = Quaternion.LookRotation(-hit.normal);
                    return true;
                }
            }

            return false;
        }


        void GetInPosition()
        {
            t += delta * 10;

            if (t > 1)
            {
                t = 1;
                inPosition = true;

                // enable the ik
                a_hook.CreatePositions(targetPos, Vector3.zero, false);
            }

            // 使用插值修改玩家位置
            Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = tp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }


        Vector3 PosWithOffset(Vector3 origin, Vector3 target)
        {
            Vector3 direction = origin - target;
            direction.Normalize();
            Vector3 offset = direction * offsetFromWall;
            return target + offset;
        }


        void LookForGround()
        {
            Vector3 origin = transform.position;
            Vector3 direction = -transform.up;

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, distanceToMoveDirection + 0.05f, ignoreLayers))
            {
                CancelClimb();
            }
        }


        void CancelClimb()
        {
            isClimbing = false;
            tpc.EnableController();

            a_hook.enabled = false;
        }

    }


    [System.Serializable]
    public class IKSnapshot
    {
        // 右手、左手、右脚、左脚
        public Vector3 rh, lh, rf, lf;
    }

}