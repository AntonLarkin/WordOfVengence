using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditDieState : BanditState
{
    public BanditDieState(BaseBandit bandit, BanditStateMachine stateMachine) : base(bandit, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        bandit.BanditAnimator.SetTrigger(bandit.IsDeadTriggerName);
        bandit.NavMeshAgent.enabled = false;
        bandit.GetComponent<Collider>().enabled = false;
        bandit.GetComponent<BaseBandit>().enabled = false;
        bandit.BanditWeapon.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        
    }
}
