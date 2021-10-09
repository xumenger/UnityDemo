using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    /**
     * 选择对应的Animator，比如Base Layer，点击齿轮，勾选【IK Pass】
     * 否则IK 不起作用
     */
    public class FreeClimbAnimHook : MonoBehaviour
    {
        Animator anim;

        IKSnapshot ikBase;
        IKSnapshot current = new IKSnapshot();
        IKSnapshot next = new IKSnapshot();
        IKGoals goals = new IKGoals();

        public float weight_rh;  // 右手
        public float weight_lh;  // 左手
        public float weight_rf;  // 右脚
        public float weight_lf;  // 左脚

        // 设置为public，以方便在面板上观察、调试
        // 如何在代码中正确的设置这几个手脚IK 的位置，是保证攀爬动作是否看起来视角效果好的关键所在！！！
        public Vector3 rh, lh, rf, lf;
        Transform helper;

        bool isMirror;
        bool isLeft;

        Vector3 prevMovDir;

        List<IKStates> ikStates = new List<IKStates>();

        float delta;
        public float lerpSpeed = 1;
        public float wallOffset = 0f;


        public void Init(FreeClimb c, Transform helper)
        {
            anim = c.anim;
            ikBase = c.baseIKsnapshot;
            this.helper = helper;
        }


        public void CreatePositions(Vector3 origin, Vector3 moveDir, bool isMid)
        {
            delta = Time.deltaTime;

            HandleAnim(moveDir, isMid);

            if (!isMid)
            {
                UpdateGoals(moveDir);
                prevMovDir = moveDir;
            }
            else
            {
                UpdateGoals(prevMovDir);
            }

            IKSnapshot ik = CreateSnapshot(origin);
            CopySnapshot(ref current, ik);

            SetIKPosition(isMid, goals.lh, current.lh, AvatarIKGoal.LeftHand);
            SetIKPosition(isMid, goals.rh, current.rh, AvatarIKGoal.RightHand);
            SetIKPosition(isMid, goals.lf, current.lf, AvatarIKGoal.LeftFoot);
            SetIKPosition(isMid, goals.rf, current.rf, AvatarIKGoal.RightFoot);

            UpdateIKWeight(AvatarIKGoal.LeftHand, 1);
            UpdateIKWeight(AvatarIKGoal.RightHand, 1);
            UpdateIKWeight(AvatarIKGoal.LeftFoot, 1);
            UpdateIKWeight(AvatarIKGoal.RightFoot, 1);
        }


        void UpdateGoals(Vector3 moveDir)
        {
            isLeft = (moveDir.x <= 0);

            if (moveDir.x != 0)
            {
                goals.lh = isLeft;
                goals.rh = !isLeft;
                goals.lf = isLeft;
                goals.rf = !isLeft;
            }
            else
            {
                bool isEnabled = isMirror;
                if (moveDir.y < 0)
                {
                    isEnabled = !isEnabled;
                }

                goals.lh = isEnabled;
                goals.rh = !isEnabled;
                goals.lf = isEnabled;
                goals.rf = !isEnabled;
            }
        }


        void HandleAnim(Vector3 moveDir, bool isMid)
        {
            if (isMid)
            {
                if (moveDir.y != 0)
                {
                    if (moveDir.x == 0)
                    {
                        isMirror = !isMirror;
                        anim.SetBool("mirror", isMirror);
                    }
                    else
                    {
                        if (moveDir.y < 0)
                        {
                            isMirror = (moveDir.x > 0);
                            anim.SetBool("mirror", isMirror);
                        }
                        else
                        {
                            isMirror = (moveDir.x < 0);
                            anim.SetBool("mirror", isMirror);
                        }
                    }

                    anim.CrossFade("climbing up wall", 0.2f);
                }
            }
            else
            {
                anim.CrossFade("hanging idle", 0.2f);
            }
        }


        public IKSnapshot CreateSnapshot(Vector3 origin)
        {
            IKSnapshot r = new IKSnapshot();

            Vector3 _lh = LocalToWorld(ikBase.lh);
            r.lh = GetPosActual(_lh, AvatarIKGoal.LeftHand);

            Vector3 _rh = LocalToWorld(ikBase.rh);
            r.rh = GetPosActual(_rh, AvatarIKGoal.RightHand);

            Vector3 _lf = LocalToWorld(ikBase.lf);
            r.lf = GetPosActual(_lf, AvatarIKGoal.LeftFoot);

            Vector3 _rf = LocalToWorld(ikBase.rf);
            r.rf = GetPosActual(_rf, AvatarIKGoal.RightFoot);

            return r;
        }



        Vector3 LocalToWorld(Vector3 position)
        {
            Vector3 r = helper.position;
            r += helper.right * position.x;
            r += helper.forward * position.z;
            r += helper.up * position.y;

            return r;
        }

        Vector3 GetPosActual(Vector3 o, AvatarIKGoal goal)
        {
            Vector3 returnPos = o;
            Vector3 origin = o;
            Vector3 dir = helper.forward;
            origin += -(dir * 0.2f);

            RaycastHit hit;
            bool isHit = false;
            if (Physics.Raycast(origin, dir, out hit, 1.5f))
            {
                Vector3 _r = hit.point + (hit.normal * wallOffset);
                returnPos = _r;
                isHit = true;

                if (goal == AvatarIKGoal.LeftFoot || goal == AvatarIKGoal.RightFoot)
                {
                    // Leg is higher than hip
                    if (hit.point.y > transform.position.y)
                    {
                        isHit = false;
                    }
                }
            }

            if (!isHit)
            {
                switch (goal)
                {
                    case AvatarIKGoal.RightHand:
                        returnPos = LocalToWorld(ikBase.rh);
                        break;
                    case AvatarIKGoal.LeftHand:
                        returnPos = LocalToWorld(ikBase.lh);
                        break;
                    case AvatarIKGoal.RightFoot:
                        returnPos = LocalToWorld(ikBase.rf);
                        break;
                    case AvatarIKGoal.LeftFoot:
                        returnPos = LocalToWorld(ikBase.lf);
                        break;
                    default:
                        break;
                }
            }

            return returnPos;
        }

        public void CopySnapshot(ref IKSnapshot to, IKSnapshot from)
        {
            to.lh = from.lh;
            to.rh = from.rh;
            to.lf = from.lf;
            to.rf = from.rf;
        }


        void SetIKPosition(bool isMid, bool isTrue, Vector3 pos, AvatarIKGoal goal)
        {
            if (isMid)
            {
                Vector3 p = GetPosActual(pos, goal);
                if (isTrue)
                {
                    UpdateIKPosition(goal, p);
                }
                else
                {
                    if (goal == AvatarIKGoal.LeftFoot || goal == AvatarIKGoal.RightFoot)
                    {
                        if (p.y > transform.position.y - 0.05f)
                        {
                            //UpdateIKPosition(goal, p);
                        }
                    }
                }
            }
            else
            {
                if (!isTrue)
                {
                    Vector3 p = GetPosActual(pos, goal);
                    UpdateIKPosition(goal, p);
                }
            }
        }


        public void UpdateIKPosition(AvatarIKGoal goal, Vector3 pos)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftHand:
                    lh = pos;
                    break;
                case AvatarIKGoal.RightHand:
                    rh = pos;
                    break;
                case AvatarIKGoal.LeftFoot:
                    lf = pos;
                    break;
                case AvatarIKGoal.RightFoot:
                    rf = pos;
                    break;
                default:
                    break;
            }
        }


        public void UpdateIKWeight(AvatarIKGoal goal, float weight)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftHand:
                    weight_lh = weight;
                    break;
                case AvatarIKGoal.RightHand:
                    weight_rh = weight;
                    break;
                case AvatarIKGoal.LeftFoot:
                    weight_lf = weight;
                    break;
                case AvatarIKGoal.RightFoot:
                    weight_rf = weight;
                    break;
                default:
                    break;
            }
        }


        // 应用Unity的Animator IK 去完善动画
        // 勾选了Animator 的IK Pass 后，会在运行中回调该方法
        private void OnAnimatorIK()
        {
            delta = Time.deltaTime;

            // 设置手脚的位置！
            SetIKPos(AvatarIKGoal.LeftHand, lh, weight_lh);
            SetIKPos(AvatarIKGoal.RightHand, rh, weight_rh);
            SetIKPos(AvatarIKGoal.LeftFoot, lf, weight_lf);
            SetIKPos(AvatarIKGoal.RightFoot, rf, weight_rf);
        }


        void SetIKPos(AvatarIKGoal goal, Vector3 targetPos, float weight)
        {
            IKStates ikState = GetIKStates(goal);
            if (ikState == null)
            {
                ikState = new IKStates();
                ikState.goal = goal;
                ikStates.Add(ikState);
            }

            if (weight == 0)
            {
                ikState.isSet = false;
            }

            if (!ikState.isSet)
            {
                ikState.position = GoalToBodyBones(goal).position;
                ikState.isSet = true;
            }

            ikState.positionWeight = weight;
            ikState.position = Vector3.Lerp(ikState.position, targetPos, delta * lerpSpeed);

            // 设置权重。value IK的权重，1代表完全使用IK值，0代表使用原动画的值
            anim.SetIKPositionWeight(goal, ikState.positionWeight);
            // 设置目标位置为ikState.position
            anim.SetIKPosition(goal, ikState.position);
        }


        Transform GoalToBodyBones(AvatarIKGoal goal)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftHand:
                    return anim.GetBoneTransform(HumanBodyBones.LeftHand);
                case AvatarIKGoal.RightFoot:
                    return anim.GetBoneTransform(HumanBodyBones.RightFoot);
                case AvatarIKGoal.LeftFoot:
                    return anim.GetBoneTransform(HumanBodyBones.LeftFoot);
                default:
                case AvatarIKGoal.RightHand:
                    return anim.GetBoneTransform(HumanBodyBones.RightHand);
            }
        }


        IKStates GetIKStates(AvatarIKGoal goal)
        {
            IKStates r = null;
            foreach (IKStates i in ikStates)
            {
                if (i.goal == goal)
                {
                    r = i;
                    break;
                }
            }

            return r;
        }


        class IKStates
        {
            public AvatarIKGoal goal;        // IK目标
            public Vector3 position;         // 设置到哪个位置
            public float positionWeight;     // 设置的权重
            public bool isSet = false;       // 是否已经设置
        }

    }


    public class IKGoals
    {
        public bool rh;
        public bool lh;
        public bool rf;
        public bool lf;
    }

}