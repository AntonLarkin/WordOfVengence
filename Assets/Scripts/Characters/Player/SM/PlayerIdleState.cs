//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerIdleState : PlayerState
//{
//    public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
//    {

//    }

//    public override void OnEnter()
//    {
//        base.OnEnter();

//        player.Animator.SetBool(player.IsWalkingBoolName, false);
//        player.Animator.SetBool(player.IsRunningBoolName, false);

//        if (player.IsAgressive)
//        {
//            player.Animator.SetBool(player.IsAttackingBoolName, true);
//            player.PlayerWapon.SetActive(true);
//        }
//        else
//        {
//            player.Animator.SetBool(player.IsAttackingBoolName, false);
//            player.PlayerWapon.SetActive(false);
//        }

//        player.NavMeshAgent.speed = player.WalkingSpeed;
//    }

//    public override void OnUpdate()
//    {

//    }

//    public override void OnExit()
//    {
//        base.OnExit();
//    }
//}
