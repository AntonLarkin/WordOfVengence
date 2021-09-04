using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditIdleState : BanditState
{
    public BanditIdleState(BaseBandit bandit, BanditStateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (bandit.IsPatroling&&bandit.IsMelee)
        {
            SetPatrolState();
        }

    }

    public override void OnUpdate()
    {
        float distance = Vector3.Distance(bandit.PlayerTransform.position, bandit.CachedTransform.position);

        if (distance < bandit.NoticeRadius && bandit.IsMelee)
        {
            SetChaseState();
        }
        else if (distance < bandit.NoticeRadius && bandit.IsLongRanged)
        {
            SetAttackState();
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

    private void SetAttackState()
    {
        stateMachine.SetState(new AttackState(bandit, stateMachine));
    }
}
