using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xum.action
{
    public class PlayerFSMManager : FSMManager
    {

        // 构造函数，初始进入eMove 状态
        public PlayerFSMManager(GameObject gameObject,
                                MonoBehaviour behaviour,
                                CharacterController controller,
                                Animator animator,
                                InputSystem inputSystem)
            : base(gameObject, behaviour, controller, animator, inputSystem, StateEnum.eMove)
        {
            
        }


        public override void InitAllFSMState()
        {
            StateMove stateMove = new StateMove(gameObject, animator, controller, this, inputSystem);
            allStateDict.Add(StateEnum.eMove, stateMove);

            StateJumpUp stateJump = new StateJumpUp(gameObject, animator, controller, this);
            allStateDict.Add(StateEnum.eJumpUp, stateJump);

            StateJumpAttack stateAttack = new StateJumpAttack(gameObject, animator, controller, this);
            allStateDict.Add(StateEnum.eJumpAttack, stateAttack);

            StateKick stateKick = new StateKick(gameObject, animator, this);
            allStateDict.Add(StateEnum.eKick, stateKick);

            StateSlashRightHand stateSlashRightHand = new StateSlashRightHand(gameObject, animator, this);
            allStateDict.Add(StateEnum.eSlashRightHand, stateSlashRightHand);

            StateClimb stateClimb = new StateClimb(gameObject, animator, controller, this);
            allStateDict.Add(StateEnum.eClimb, stateClimb);
        }
    }
}