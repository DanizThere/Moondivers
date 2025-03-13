using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FEChaseState : FSMState
{
    FSM fsm;
    Transform transform;
    LayerMask target;
    NavMeshAgent agent;
    Animator animator;
    private int radius = 30;
    public FEChaseState(FSM fsm, Transform transform, LayerMask target, NavMeshAgent agent, Animator animator) : base(fsm)
    {
        this.animator = animator;
        this.transform = transform;
        this.target = target;
        this.agent = agent;
    }

    public override void Update()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, target);
        if (targets.Length < 1) return;
        float dist1;
        dist1 = Vector3.Distance(transform.position, targets[0].transform.position);
        foreach(Collider target in targets)
        {           
            float dst2 = Vector3.Distance(transform.position, target.transform.position);
            if (dst2 <= dist1)
            {
                agent.SetDestination(target.transform.position);
            }
        }
        if(targets.Length < 1)
        {
            Fsm.SetState<FEIdleState>();
        }


        if (agent.velocity.magnitude < .1f)
        {
            Fsm.SetState<FEAttackState>();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
        Debug.Log(agent.velocity.magnitude);
    }

    public override void Exit()
    {
        Debug.Log("im exited");
    }
}
