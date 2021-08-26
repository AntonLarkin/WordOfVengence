using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    private State currentState;

    public void UpdateState()
    {
        currentState?.OnUpdate();
    }

    public void SetState(State newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
