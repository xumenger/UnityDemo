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

        public float w_rh;  // 右手
        public float w_lh;  // 左手
        public float w_rf;  // 右脚
        public float w_lf;  // 左脚

        // 设置为public，以方便在面板上观察、调试
        // 如何在代码中正确的设置这几个手脚IK 的位置，是保证攀爬动作是否看起来视角效果好的关键所在！！！
        public Vector3 rh, lh, rf, lf;
        Transform h;

        bool isMirror;
        bool isLeft;

        Vector3 prevMovDir;

        float delta;
        public float lerpSpeed = 1;


        public void Init(FreeClimb c, Transform helper)
        {
            anim = c.anim;
            ikBase = c.baseIKsnapshot;
            h = helper;
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

            SetIKPosition(isMid, goals.lf, current.lf, AvatarIKGoal.LeftFoot);
            SetIKPosition(isMid, goals.rf, current.rf, AvatarIKGoal.RightFoot);
            SetIKPosition(isMid, goals.lh, current.lh, AvatarIKGoal.LeftHand);
            SetIKPosition(isMid, goals.rh, current.rh, AvatarIKGoal.RightHand);

            UpdateIKWeight(AvatarIKGoal.RightHand, 1);
            UpdateIKWeight(AvatarIKGoal.LeftHand, 1);
            UpdateIKWeight(AvatarIKGoal.RightFoot, 1);
            UpdateIKWeight(AvatarIKGoal.LeftFoot, 1);
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


        public IKSnapshot CreateSnapshot(Vector3 o)
        {
            IKSnapshot r = new IKSnapshot();

            Vector3 _rh = LocalToWorld(ikBase.rh);
            r.rh = GetPosActual(_rh, AvatarIKGoal.RightHand);

            Vector3 _lh = LocalToWorld(ikBase.lh);
            r.lh = GetPosActual(_lh, AvatarIKGoal.LeftHand);

            Vector3 _rf = LocalToWorld(ikBase.rf);
            r.rf = GetPosActual(_rf, AvatarIKGoal.RightFoot);

            Vector3 _lf = LocalToWorld(ikBase.lf);
            r.lf = GetPosActual(_lf, AvatarIKGoal.LeftFoot);

            return r;
        }


        public float wallOffset = 0.0f;

        Vector3 GetPosActual(Vector3 o, AvatarIKGoal goal)
        {
            Vector3 r = o;
            Vector3 origin = o;
            Vector3 dir = h.forward;
            origin += -(dir * 0.2f);

            RaycastHit hit;
            bool isHit = false;
            if (Physics.Raycast(origin, dir, out hit, 1.5f))
            {
                Vector3 _r = hit.point + (hit.normal * wallOffset);
                r = _r;
                isHit = true;

                if (goal == AvatarIKGoal.LeftFoot || goal == AvatarIKGoal.RightFoot)
                {
                    if (hit.point.y > transform.position.y - 0.1f)
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
                        r = LocalToWorld(ikBase.rh);
                        break;
                    case AvatarIKGoal.LeftHand:
                        r = LocalToWorld(ikBase.lh);
                        break;
                    case AvatarIKGoal.RightFoot:
                        r = LocalToWorld(ikBase.rf);
                        break;
                    case AvatarIKGoal.LeftFoot:
                        r = LocalToWorld(ikBase.lf);
                        break;
                    default:
                        break;
                }
            }

            return r;
        }

        Vector3 LocalToWorld(Vector3 p)
        {
            Vector3 r = h.position;
            r += h.right * p.x;
            r += h.forward * p.z;
            r += h.up * p.y;

            return r;
        }

        public void CopySnapshot(ref IKSnapshot to, IKSnapshot from)
        {
            to.rh = from.rh;
            to.lh = from.lh;
            to.rf = from.rf;
            to.lf = from.lf;
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
                        if (p.y > transform.position.y - 0.25f)
                        {
                            UpdateIKPosition(goal, p);
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
            switch(goal)
            {
                case AvatarIKGoal.RightHand:
                    rh = pos;
                    break;
                case AvatarIKGoal.LeftHand:
                    lh = pos;
                    break;
                case AvatarIKGoal.RightFoot:
                    rf = pos;
                    break;
                case AvatarIKGoal.LeftFoot:
                    lf = pos;
                    break;
                default:
                    break;
            }
        }


        public void UpdateIKWeight(AvatarIKGoal goal, float w)
        {
            switch (goal)
            {
                case AvatarIKGoal.RightHand:
                    w_rh = w;
                    break;
                case AvatarIKGoal.LeftHand:
                    w_lh = w;
                    break;
                case AvatarIKGoal.RightFoot:
                    w_rf = w;
                    break;
                case AvatarIKGoal.LeftFoot:
                    w_lf = w;
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
            SetIKPos(AvatarIKGoal.RightHand, rh, w_rh);
            SetIKPos(AvatarIKGoal.LeftHand, lh, w_lh);
            SetIKPos(AvatarIKGoal.RightFoot, rf, w_rf);
            SetIKPos(AvatarIKGoal.LeftFoot, lf, w_lf);
        }

        void SetIKPos(AvatarIKGoal goal, Vector3 tp, float w)
        {
            IKStates ikState = GetIKStates(goal);
            if (ikState == null)
            {
                ikState = new IKStates();
                ikState.goal = goal;
                ikStates.Add(ikState);
            }

            if (w == 0)
            {
                ikState.isSet = false;
            }

            if (!ikState.isSet)
            {
                ikState.position = GoalToBodyBones(goal).position;
                ikState.isSet = true;
            }

            ikState.positionWeight = w;
            ikState.position = Vector3.Lerp(ikState.position, tp, delta * lerpSpeed);

            // 设置权重。value IK的权重，1代表完全使用IK值，0代表使用原动画的值
            anim.SetIKPositionWeight(goal, ikState.positionWeight);
            // 设置目标位置为ikState.position
            anim.SetIKPosition(goal, ikState.position);
        }


        Transform GoalToBodyBones(AvatarIKGoal goal)
        {
            switch(goal)
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
            foreach(IKStates i in ikStates)
            {
                if (i.goal == goal)
                {
                    r = i;
                    break;
                }
            }

            return r;
        }


        List<IKStates> ikStates = new List<IKStates>();

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