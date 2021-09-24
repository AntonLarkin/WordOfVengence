using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuest 
{
    public enum QuestProgression
    {
        NotAvailable,
        Available,
        Accepted,
        InProgress,
        Completed,
        Done
    }

    public string questTitle;
    public int questId;
    public QuestProgression questProgression;
    public string questDescription;
    public string acomplishmentMessage;

    public string questObjective;
    public ScriptableObject questItem;
    public GameObject[] killTargets;
    public int questObjectiveCount;
    public int questObjectiveRequirment;

    public ScriptableObject questReward;

}
