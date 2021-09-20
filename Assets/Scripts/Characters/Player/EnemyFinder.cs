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

    private void OnValidate()
    {
        DetectEnemiesOnLevel();
        player = FindObjectOfType<Player>();
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
        if (bandits != null)
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
    }

}
