using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FEAttackState : FSMState
{
    Animator animator;
    public FEAttackState(FSM fsm, Animator animator) : base(fsm)
    {
        this.animator = animator;
    }

    public override void Update()
    {
        Attack();
    }

    private void Attack()
    {
        animator.SetTrigger("FirstAttack");
    }
}
