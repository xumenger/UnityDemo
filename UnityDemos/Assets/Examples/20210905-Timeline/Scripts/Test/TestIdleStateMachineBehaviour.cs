using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdleStateMachineBehaviour : StateMachineBehaviour
{
    public GameObject particle;
    public float radius;
    public float power;

    protected GameObject clone;

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Idle OnStateMachineEnter: " + animator.name + " - " + stateMachinePathHash);
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("Idle OnStateMachineExit: " + animator.name + " - " + stateMachinePathHash);
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle OnStateEnter: " + animator.name + " - " + stateInfo + " - " + layerIndex);

        //clone = Instantiate(particle, animator.rootPosition, Quaternion.identity) as GameObject;
        //Rigidbody rb = clone.GetComponent<Rigidbody>();
        //rb.AddExplosionForce(power, animator.rootPosition, radius, 3.0f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle OnStateExit: " + animator.name + " - " + stateInfo + " - " + layerIndex);

        //Destroy(clone);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle OnStateUpdate: " + animator.name + " - " + stateInfo + " - " + layerIndex);

        // Debug.Log("On Attack Update ");
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle OnStateMove: " + animator.name + " - " + stateInfo + " - " + layerIndex);

        //Debug.Log("On Attack Move ");
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle OnStateIK: " + animator.name + " - " + stateInfo + " - " + layerIndex);

        // Debug.Log("On Attack IK ");
    }
}
