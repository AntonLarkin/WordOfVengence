using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;

    public List<BaseQuest> quests = new List<BaseQuest>();
    public List<BaseQuest> currentQuests = new List<BaseQuest>();

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

    public void AcceptQuest(int questId)
    {
        for(int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questId == questId && quests[i].questProgression == BaseQuest.QuestProgression.Available)
            {
                currentQuests.Add(quests[i]);
                currentQuests[i].questProgression = BaseQuest.QuestProgression.Accepted;
            }
        }
    }

    public void CompleteQuest(int questId)
    {
        for(int i = 0; i < currentQuests.Count; i++)
        {
            if(currentQuests[i].questId==questId && currentQuests[i].questProgression == BaseQuest.QuestProgression.Completed)
            {
                currentQuests[i].questProgression = BaseQuest.QuestProgression.Done;
                currentQuests.Remove(currentQuests[i]);
            }
        }
    }

    public void AddQuestItem(int questId,int itemAmount)
    {
        for(int i = 0; i < currentQuests.Count; i++)
        {
            if(currentQuests[i].questId == questId && currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
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

    public bool RequestAvailableQuest(int questId)
    {
        for(int i = 0; i < quests.Count; i++)
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
            if (quests[i].questId == questId && quests[i].questProgression == BaseQuest.QuestProgression.Completed)
            {
                return true;
            }
        }
        return false;
    }

}
