using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgressiveState : PlayerState
{
    public PlayerAgressiveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.Animator.SetTrigger(player.EnemyNearbyTriggerName);
        player.PlayerWapon.SetActive(true);

    }

    public override void OnUpdate()
    {
        if (Vector3.Distance(player.CachedTransform.position, player.GetDestinationPoint()) <= player.MinDistance)
        {
            stateMachine.SetState(new PlayerIdleState(player, stateMachine));
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        player.PlayerWapon.SetActive(false);
    }

}
