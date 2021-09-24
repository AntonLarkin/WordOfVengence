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
        bandit.BanditWeapon.gameObject.SetActive(false);
        QuestDeadEnemyCounter();
    }

    public override void OnUpdate()
    {

    }

    private void QuestDeadEnemyCounter()
    {
        var qm = QuestManager.questManager;
        for (int i = 0; i < qm.currentQuests.Count; i++)
        {
            for (int j = 0; j < qm.currentQuests[i].killTargets.Length; j++)
            {
                if (bandit.gameObject == qm.currentQuests[i].killTargets[j])
                {
                    if (qm.currentQuests[i].questObjectiveCount < qm.currentQuests[i].questObjectiveRequirment)
                    {
                        qm.IncreaseDeadEnemyCount(qm.currentQuests[i].questId, 1);
                    }
                    else
                    {
                        qm.CompleteQuest(qm.currentQuests[i].questId);
                    }
                }
            }
        }
    }

}
