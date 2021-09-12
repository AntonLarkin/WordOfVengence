using System.Collections;
using System;
using UnityEngine;

public class PatrolState : BanditState
{
    public PatrolState(BaseBandit bandit, BanditStateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    private int currentPatrolPoint;
    private Vector3 patrolPosition;

    public override void OnEnter()
    {
        base.OnEnter();

        if (bandit.NavMeshAgent.enabled == false)
        {
            bandit.NavMeshAgent.enabled = true;
        }

        SetStartPatrolPosition();
        SetStartMovement();
    }

    public override void OnUpdate()
    {
        Patrol();

        float distance = Vector3.Distance(bandit.PlayerTransform.position, bandit.CachedTransform.position);

        if (distance < bandit.NoticeRadius&&bandit.IsMelee&&bandit.Player.IsAlive)
        {
            SetChaseState();
        }
        else if(distance < bandit.NoticeRadius && bandit.IsLongRanged && bandit.Player.IsAlive)
        {
            SetAttackState();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void SetChaseState()
    {
        stateMachine.SetState(new ChaseState(bandit, stateMachine));
    }

    private void SetAttackState()
    {
        stateMachine.SetState(new AttackState(bandit, stateMachine));
    }

    private void SetStartPatrolPosition()
    {
        currentPatrolPoint = 0;
        patrolPosition = bandit.PatrolPointsTransform[currentPatrolPoint].position;
    }

    private void SetStartMovement()
    {
        bandit.BanditAnimator.SetBool(bandit.IsPatrolingBoolName, true);
        bandit.NavMeshAgent.speed = bandit.WalkingSpeed;
        bandit.NavMeshAgent.SetDestination(patrolPosition);
    }

    private void Patrol()
    {
        if (Vector3.Distance(bandit.CachedTransform.position, patrolPosition) <= bandit.MinDistance)
        {
            CalculateNextPatrolPoint();

            StopWhilePatroling();
        }
    }

    private void CalculateNextPatrolPoint()
    {
        currentPatrolPoint++;

        if (currentPatrolPoint >= bandit.PatrolPointsTransform.Length)
        {
            currentPatrolPoint = 0;
        }

        patrolPosition = bandit.PatrolPointsTransform[currentPatrolPoint].position;
    }

    private void StopWhilePatroling()
    {
        bandit.BanditAnimator.SetBool(bandit.IsPatrolingBoolName, false);

        bandit.StartCoroutine(OnPatrolStop());
    }

    private IEnumerator OnPatrolStop()
    {
        yield return new WaitForSeconds(3f);

        bandit.BanditAnimator.SetBool(bandit.IsPatrolingBoolName, true);
        bandit.NavMeshAgent.SetDestination(patrolPosition);

    }
}
