using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTheValley : Quest
{
    private void Start()
    {
        QuestName = "Clear the valley from bandits!";
        Description = "There are rumors about groups of bandits walking around your usual hunting place. " +
            "If you run into someone - just kill them, they are not supposed to be here!";

        Goals.Add(new KillGoal(this, 1, "Kill 1 bandits", false, 0, 1));
        Goals.Add(new KillGoal(this, 2, "Kill 1 archer", false, 0, 1));

        Goals.ForEach(g => g.Init());
    }
}
