using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;

    public List<BaseQuest> quests = new List<BaseQuest>();
    public List<BaseQuest> currentQuests = new List<BaseQuest>();

    public Inventory inventory;


    private void Awake()
    {
        if (questManager == null)
        {
            questManager = this;
        }
        else if (questManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        for(int i = 0; i < currentQuests.Count; i++)
        {
            if(currentQuests[i].questObjectiveCount>= currentQuests[i].questObjectiveRequirment &&
                currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                CompleteQuest(currentQuests[i].questId);
            }
        }
    }

    public void AcceptQuest(int questId)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questId == questId && quests[i].questProgression == BaseQuest.QuestProgression.Available)
            {
                currentQuests.Add(quests[i]);
                for (int j = 0; j < currentQuests.Count; j++)
                {
                    currentQuests[j].questProgression = BaseQuest.QuestProgression.Accepted;
                }
            }
        }
    }

    public void CompleteQuest(int questId)
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].questId == questId && currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                currentQuests[i].questProgression = BaseQuest.QuestProgression.Completed;
            }
        }
    }

    public void DoneQuest(int questId)
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].questId == questId && currentQuests[i].questProgression == BaseQuest.QuestProgression.Completed)
            {
                if (currentQuests[i].questReward != null && currentQuests[i].questReward is Item)
                {
                    inventory.IsAbleToAddItem((Item)currentQuests[i].questReward);
                }
                currentQuests.Remove(currentQuests[i]);
            }
        }
    }

    public void AddQuestItem(int questId, int itemAmount)
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].questId == questId && currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                currentQuests[i].questObjectiveCount += itemAmount;
            }

            if (currentQuests[i].questObjectiveCount >= currentQuests[i].questObjectiveRequirment &&
                currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                currentQuests[i].questProgression = BaseQuest.QuestProgression.Completed;

            }
        }
    }

    public void IncreaseDeadEnemyCount(int questId, int killCountAmount)
    {
        for (int i = 0; i < currentQuests.Count; i++)
        {
            if (currentQuests[i].questId == questId && currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                currentQuests[i].questObjectiveCount += killCountAmount;
            }
            if (currentQuests[i].questObjectiveCount >= currentQuests[i].questObjectiveRequirment &&
                currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                currentQuests[i].questProgression = BaseQuest.QuestProgression.Completed;

            }
        }
    }

    public void QuestRequest(QuestGiver questGiver)
    {
        if (questGiver.availableQuestIDs.Count > 0)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                for (int j = 0; j < questGiver.availableQuestIDs.Count; j++)
                {
                    if (quests[i].questId == questGiver.availableQuestIDs[j] && quests[i].questProgression == BaseQuest.QuestProgression.Available)
                    {
                        QuestUIManager.questUiManager.isQuestAvailable = true;
                        QuestUIManager.questUiManager.availableQuests.Add(quests[i]);
                    }
                }
            }

            for (int i = 0; i < currentQuests.Count; i++)
            {
                for (int j = 0; j < questGiver.receivableQuestIDs.Count; j++)
                {
                    if (currentQuests[i].questId == questGiver.receivableQuestIDs[j] &&
                        currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
                    {
                        QuestUIManager.questUiManager.isQuestRunning = true;
                        QuestUIManager.questUiManager.activeQuests.Add(quests[i]);
                    }
                }
            }
        }
    }

    public bool RequestAvailableQuest(int questId)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questId == questId && quests[i].questProgression == BaseQuest.QuestProgression.Available)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptedQuest(int questId)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questId == questId && quests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestCompletedQuest(int questId)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questId == questId && (quests[i].questProgression == BaseQuest.QuestProgression.Completed))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckAvailableQuests(QuestGiver questGiver)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            for (int j = 0; j < questGiver.availableQuestIDs.Count; j++)
            {
                if (quests[i].questId == questGiver.availableQuestIDs[j] && quests[i].questProgression == BaseQuest.QuestProgression.Available)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckAcceptedQuests(QuestGiver questGiver)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            for (int j = 0; j < questGiver.receivableQuestIDs.Count; j++)
            {
                if (quests[i].questId == questGiver.receivableQuestIDs[j] && quests[i].questProgression == BaseQuest.QuestProgression.Accepted)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckCompletedQuests(QuestGiver questGiver)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            for (int j = 0; j < questGiver.receivableQuestIDs.Count; j++)
            {
                if (quests[i].questId == questGiver.receivableQuestIDs[j] && quests[i].questProgression == BaseQuest.QuestProgression.Completed)
                {
                    return true;
                }
            }
        }
        return false;
    }


}
