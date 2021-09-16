using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal 
{
    public Quest Quest { get; set; }
    public string Description { get; set; }
    public bool IsComleted { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public virtual void Init()
    {
        //default init
    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        Quest.CheckGoals();
        IsComleted = true;
    }

}