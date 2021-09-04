using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.Animator.SetTrigger(player.IsDeadTriggerName);
        player.NavMeshAgent.enabled = false;
        player.GetComponent<Collider>().enabled = false;

    }

    public override void OnUpdate()
    {

    }
}
