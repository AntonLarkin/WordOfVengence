using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Effect", menuName = "Inventory/Effects/Healing")]
public class HealingEffect : UsableItemEffect
{
    public int HealthAmount;

    public override void ExecuteEffect(UsableItem usableItem, Player player)
    {
        player.HealPlayer(HealthAmount);
    }

    public override string GetDescription()
    {
        return "Heals for " + HealthAmount + " health points.";
    }
}
