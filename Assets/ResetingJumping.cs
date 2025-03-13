using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetingJumping : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
    }
}
