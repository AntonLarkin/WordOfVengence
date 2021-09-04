using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BanditState

{
    public AttackState(BaseBandit bandit, BanditStateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (bandit.IsLongRanged)
        {
            bandit.BanditAnimator.SetBool(bandit.IsPlayerFoundBoolName, true);
        }

        if (bandit.IsMelee)
        {
            bandit.BanditAnimator.SetBool(bandit.IsFollowingBoolName, false);
        }

        bandit.BanditAnimator.SetTrigger(bandit.AttackSimpleTriggerName);

        bandit.NavMeshAgent.enabled = false;
    }
    public override void OnUpdate()
    {
        bandit.CachedTransform.LookAt(bandit.PlayerTransform.position);

        float distance = Vector3.Distance(bandit.PlayerTransform.position, bandit.CachedTransform.position);

        if (distance > bandit.AttackRadius && distance < bandit.ChaseRadius&&bandit.IsMelee)
        {
            SetChaseState();
        }
        else if(distance > bandit.AttackRadius && bandit.IsLongRanged)
        {
            bandit.BanditAnimator.SetBool(bandit.IsPlayerFoundBoolName, false);
            bandit.NavMeshAgent.enabled = true;
            bandit.BanditAnimator.SetBool(bandit.IsPatrolingBoolName, true);
            SetPatrolState();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

    }

    private void SetChaseState()
    {
        bandit.BanditAnimator.SetBool(bandit.IsFollowingBoolName, true);
        bandit.NavMeshAgent.enabled = true;

        stateMachine.SetState(new ChaseState(bandit, stateMachine));
    }

    private void SetPatrolState()
    {
        stateMachine.SetState(new PatrolState(bandit, stateMachine));
    }
}
