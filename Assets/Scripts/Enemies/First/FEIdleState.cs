using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FEIdleState : FSMState
{
    private FSM fsm;
    private Transform transform;

    private float timerToDo = 1;
    private float timer = 0;

    private int radius = 30;
    protected float angle = 45;
    private LayerMask target;
    NavMeshAgent agent;
    Animator animator;
    public FEIdleState(FSM fsm, Transform transform, LayerMask target, NavMeshAgent agent, Animator animator) : base(fsm)
    {
        this.transform = transform;
        this.target = target;
        this.agent = agent;
        this.animator = animator;
    }
    public override void Enter()
    {
    }
    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerToDo)
        {
            RandomMove();
        }

        FindTargets();

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void RandomMove()
    {
        Vector3 toMove = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5));      

        agent.SetDestination(toMove);

        transform.LookAt(toMove);      

        UpdateNumber();
    }

    private void UpdateNumber()
    {
        timer = 0;
        timerToDo = Random.Range(0, 15);       
    }

    void FindTargets()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, target);

        for(int i = 0; i < targets.Length; i++)
        {
            Transform target = targets[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < angle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget))
                {
                    Fsm.SetState<FEChaseState>();
                }
            }
        }
    }
}
