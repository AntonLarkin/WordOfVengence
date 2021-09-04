using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BanditState
{
    public ChaseState(BaseBandit bandit, BanditStateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        TriggerEnemy(true);

        bandit.NavMeshAgent.speed = bandit.RunningSpeed;
    }

    public override void OnUpdate()
    {
        float distance = Vector3.Distance(bandit.PlayerTransform.position, bandit.CachedTransform.position);

        if (distance < bandit.AttackRadius)
        {
            SetAttackState();
        }
        else if(distance> bandit.AttackRadius&&distance<bandit.ChaseRadius)
        {
            bandit.NavMeshAgent.SetDestination(bandit.PlayerTransform.position);
        }
        else
        {
            SetPatrolState();
        }
    }

    private void TriggerEnemy(bool isPlayerVisible)
    {
        bandit.BanditAnimator.SetBool(bandit.IsPlayerFoundBoolName, isPlayerVisible);
        bandit.BanditWeapon.gameObject.SetActive(isPlayerVisible);
    }

    private void SetAttackState()
    {
        stateMachine.SetState(new AttackState(bandit, stateMachine));
    }

    private void SetPatrolState()
    {
        TriggerEnemy(false);

        stateMachine.SetState(new PatrolState(bandit, stateMachine));
    }
}
