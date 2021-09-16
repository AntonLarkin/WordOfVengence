using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject quests;
    [SerializeField] private string questType;

    public Quest Quest { get; set; }
    public bool AssignedQuest { get; set; }
    public bool QuestFinished { get; set; }
    private void Interract()
    {
        if(!AssignedQuest && !QuestFinished)
        {
            AssignQuest();
        }
        else if (AssignedQuest && !QuestFinished)
        {
            CheckQuest();
        }
    }

    private void AssignQuest()
    {
        AssignedQuest = true;
        Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
        
    }

    private void CheckQuest()
    {
        if (Quest.IsCompleted)
        {
            QuestFinished = true;
            AssignedQuest = false;
        }
    }

}
