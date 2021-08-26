using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(Bandit bandit, StateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (bandit.IsPatroling)
        {
            SetPatrolState();
        }

    }

    public override void OnUpdate()
    {
        float distance = Vector3.Distance(bandit.PlayerTransform.position, bandit.CachedTransform.position);

        if (distance < bandit.NoticeRadius)
        {
            SetChaseState();
        }
    }

    private void SetPatrolState()
    {
        stateMachine.SetState(new PatrolState(bandit, stateMachine));
    }

    private void SetChaseState()
    {
        stateMachine.SetState(new ChaseState(bandit, stateMachine));
    }
}
