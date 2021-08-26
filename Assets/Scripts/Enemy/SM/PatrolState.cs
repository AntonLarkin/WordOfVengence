using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(Bandit bandit, StateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    private int currentPatrolPoint;
    private Vector3 patrolPosition;

    public override void OnEnter()
    {
        base.OnEnter();

        currentPatrolPoint = 0;
        patrolPosition = bandit.PatrolPointsTransform[currentPatrolPoint].position;
        bandit.BanditAnimator.SetBool(bandit.IsPatrolingBoolName, true);
        bandit.NavMeshAgent.SetDestination(patrolPosition);
    }

    public override void OnUpdate()
    {
        Debug.Log(patrolPosition);
        Patrol();

        float distance = Vector3.Distance(bandit.PlayerTransform.position, bandit.CachedTransform.position);

        if (distance < bandit.NoticeRadius)
        {
            SetChaseState();
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

    private void Patrol()
    {
        if (Vector3.Distance(bandit.CachedTransform.position, patrolPosition) <= bandit.MinDistance)
        {
            CalculateNextPatrolPoint();
            bandit.NavMeshAgent.SetDestination(patrolPosition);
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
}
