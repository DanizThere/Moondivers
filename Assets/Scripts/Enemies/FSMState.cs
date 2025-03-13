using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState
{
    protected readonly FSM Fsm;
    public FSMState(FSM fsm)
    {
        Fsm = fsm;
    }

    public virtual void Enter()
    {

    }
    public virtual void Exit()
    {

    }
    public virtual void Update() { }
}
