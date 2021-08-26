using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected readonly Bandit bandit;
    protected readonly StateMachine stateMachine;

    protected State(Bandit bandit,StateMachine stateMachine)
    {
        this.bandit = bandit;
        this.stateMachine = stateMachine;
    }
         
    public abstract void OnUpdate();
    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }



}
