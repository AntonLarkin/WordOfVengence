//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerAttackState : PlayerState
//{
//    public PlayerAttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
//    {

//    }

//    public override void OnEnter()
//    {
//        base.OnEnter();

//        player.NavMeshAgent.enabled = false;
//        player.PlayerWapon.SetActive(true);
//        player.Animator.SetBool(player.IsWalkingBoolName, false);
//        player.Animator.SetBool(player.IsRunningBoolName, false);
//        player.Animator.SetTrigger(player.AttackTriggerName);
//    }

//    public override void OnUpdate()
//    {
//        player.CachedTransform.LookAt(player.CurrentBandit.CachedTransform.position);

//        if (player.CurrentBandit.GetComponent<BaseBandit>().CurrentHealth <= 0)
//        {

//            //player.LeaveEnemy();
//        }

//        if (Vector3.Distance(player.CachedTransform.position, player.GetDestinationPoint()) >= player.AttackDistance)
//        {
//            Debug.Log("trying to escape");
//            player.Escape();
//            stateMachine.SetState(new PlayerMovingState(player, stateMachine));
//        }
//        else
//        {
//            Debug.Log("too close");
//        }

//    }

//    public override void OnExit()
//    {
//        base.OnExit();

//        player.NavMeshAgent.enabled = true;

//        player.Animator.SetBool(player.IsWalkingBoolName, true);
//        player.Animator.SetBool(player.IsRunningBoolName, true);
//    }

//}
