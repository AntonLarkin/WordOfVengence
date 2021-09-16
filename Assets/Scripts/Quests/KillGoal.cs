using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyID { get; set; }
    private BaseBandit baseBandit;

    public KillGoal(Quest quest,int enemyID, string description, bool isCompleted, int currentAmount,int requiredAmount)
    {
        this.Quest = quest;
        this.EnemyID = enemyID;
        this.Description = description;
        this.IsComleted = isCompleted;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();

        baseBandit.OnBanditDie += EnemyDied;
    }

    private void EnemyDied(BaseBandit bandit)
    {
        if(bandit.ID == this.EnemyID)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
