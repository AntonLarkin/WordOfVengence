using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExpirienceReward { get; set; }
    public bool IsCompleted { get; set; }

    public void CheckGoals()
    {
        if (Goals.All(goal => goal.IsComleted))
        {
            Complete();
        }
    }

    private void Complete()
    {
        IsCompleted = true;
        GiveReward();
    }

    private void GiveReward()
    {
        Debug.Log("Here is your reward!");
    }
}
