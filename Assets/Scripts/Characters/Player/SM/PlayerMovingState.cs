//using System.Collections;
//using System;
//using UnityEngine;

//public class PlayerMovingState : PlayerState
//{
//    public PlayerMovingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
//    {

//    }

//    public override void OnEnter()
//    {
//        base.OnEnter();

//        if (!player.IsAgressive)
//        {
//            player.Animator.SetBool(player.IsWalkingBoolName, true);
//            player.Animator.SetBool(player.IsAttackingBoolName, false);
//        }
//        else
//        {
//            player.Animator.SetBool(player.IsWalkingBoolName, true);
//            player.Animator.SetBool(player.IsAttackingBoolName, true);
//            player.PlayerWapon.SetActive(true);
//        }

//        player.NavMeshAgent.SetDestination(player.GetDestinationPoint());
//    }

//    public override void OnUpdate()
//    {
//        if (Vector3.Distance(player.CachedTransform.position, player.GetDestinationPoint()) <= player.MinDistance)
//        {
//            stateMachine.SetState(new PlayerIdleState(player, stateMachine));
//        }
//    }

//    public override void OnExit()
//    {
//        base.OnExit();
//    }
//}
