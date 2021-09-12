using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyFinder : MonoBehaviour
{
    [SerializeField] private float triggerDistance;
    [SerializeField] private float escapeDistance;
    private BaseBandit[] bandits;
    private BaseBandit closestBandit;
    private float closestEnemyDistance;

    private Player player;

    public BaseBandit ClosestBandit => closestBandit;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        DetectEnemiesOnLevel();
    }

    private void Update()
    {
        if (closestBandit != null)
        {
            PlayerEscape();
            DetectDeadEnemies();
            UpdateClosestDistance();
            return;
        }

        FindClosestEnemie();

    }

    private void DetectDeadEnemies()
    {
        if (closestBandit.CurrentHealth <= 0)
        {
            closestBandit = null;
        }
    }

    private void DetectEnemiesOnLevel()
    {
        bandits = FindObjectsOfType<BaseBandit>();
    }

    private void FindClosestEnemie()
    {
        closestEnemyDistance = triggerDistance;

        foreach (BaseBandit bandit in bandits)
        {
            var distance = Vector3.Distance(transform.position, bandit.CachedTransform.position);

            if (distance <= closestEnemyDistance&&bandit.CurrentHealth>0)
            {
                closestBandit = bandit;
                closestEnemyDistance = distance;

                player.SetPlayerAgressive(true);
            }
            
        }

        if (closestBandit == null)
        {
            CalmDownPlayer();
        }
    }

    private void CalmDownPlayer()
    {
        closestBandit = null; 
        player.SetPlayerAgressive(false);
    }

    private void PlayerEscape()
    {
        if(Vector3.Distance(transform.position, closestBandit.CachedTransform.position) > escapeDistance)
        {
            CalmDownPlayer();
        }
    }

    private void UpdateClosestDistance()
    {
        closestEnemyDistance = Vector3.Distance(transform.position, ClosestBandit.CachedTransform.position);

        foreach (BaseBandit bandit in bandits)
        {
            var distance = Vector3.Distance(transform.position, bandit.CachedTransform.position);

            if (distance <= closestEnemyDistance && bandit.CurrentHealth > 0)
            {
                closestBandit = bandit;
                closestEnemyDistance = distance;

                player.SetPlayerAgressive(true);
            }

        }
    }



    //private void CheckForEnemies()
    //{
    //    if (bandits.Length > 0)
    //    {
    //        foreach (BaseBandit bandit in bandits)
    //        {
    //            if (Vector3.Distance(transform.position, bandit.transform.position) <= triggerDistance)
    //            {
    //                if (Vector3.Distance(transform.position, bandit.transform.position) <= attackDistance && !IsEscaping)
    //                {
    //                    SetArgessive(true);
    //                    Debug.Log("trying to escape");
    //                    currentBandit = bandit;
    //                    if (currentBandit.CurrentHealth > 0)
    //                    {
    //                        Debug.Log(CurrentBandit);
    //                        stateMachine.SetState(new PlayerAttackState(this, stateMachine));
    //                    }
    //                    else
    //                    {
    //                        currentBandit = null;
    //                    }

    //                }
    //                else
    //                {
    //                    SetArgessive(true);
    //                    stateMachine.SetState(new PlayerAgressiveState(this, stateMachine));
    //                }

    //            }
    //            else
    //            {
    //                stateMachine.SetState(new PlayerMovingState(this, stateMachine));
    //                SetArgessive(false);
    //            }
    //        }
    //    }

    //}
}
