using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20211007
{
    public class EnemyAIFSMManager : FSMManager
    {
        // 构造函数，初始进入eMove 状态
        public EnemyAIFSMManager(Transform transform,
                                MonoBehaviour behaviour,
                                CharacterController controller,
                                Animator animator,
                                InputSystem inputSystem)
            : base(transform, behaviour, controller, animator, inputSystem, StateEnum.eMove)
        {

        }


        public override void InitAllFSMState()
        {
            StateMove stateMove = new StateMove(transform, animator, controller, this, inputSystem);
            allStateDict.Add(StateEnum.eMove, stateMove);

            StateJumpUp stateJump = new StateJumpUp(transform, animator, controller, this);
            allStateDict.Add(StateEnum.eJumpUp, stateJump);

            StateJumpAttack stateAttack = new StateJumpAttack(transform, animator, controller, this);
            allStateDict.Add(StateEnum.eJumpAttack, stateAttack);

            StateKick stateKick = new StateKick(transform, animator, this);
            allStateDict.Add(StateEnum.eKick, stateKick);

            StateSlashRightHand stateSlashRightHand = new StateSlashRightHand(transform, animator, this);
            allStateDict.Add(StateEnum.eSlashRightHand, stateSlashRightHand);
        }
    }

}