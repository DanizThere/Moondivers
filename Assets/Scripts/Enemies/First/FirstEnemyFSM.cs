using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstEnemyFSM : MonoBehaviour
{
    private FSM fsm;
    Animator animator;
    public LayerMask target;
    NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fsm = new FSM();

        fsm.AddState(new FEIdleState(fsm, transform, target, agent, animator));
        fsm.AddState(new FEChaseState(fsm, transform, target, agent, animator));
        fsm.SetState<FEIdleState>();
    }

    private void Update()
    {
        fsm.Update();
    }
}
