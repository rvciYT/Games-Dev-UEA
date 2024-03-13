using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackig : StateMachineBehaviour
{
    public string boolParameterName = "IsAttacking";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolParameterName, true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolParameterName, false);
    }
}
