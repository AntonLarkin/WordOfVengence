using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Inventory/Usable Item")]
public class UsableItem : Item
{
    public List<UsableItemEffect> Effects;
    public bool IsConsumable;
    public virtual void Use(Player player)
    {
        foreach(UsableItemEffect effect in Effects)
        {
            effect.ExecuteEffect(this, player);
        }
    }

    public override string GetItemType()
    {
        return IsConsumable ? "Consumable" : "Usable";
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        foreach(UsableItemEffect effect in Effects)
        {
            sb.AppendLine(effect.GetDescription());
        }

        return sb.ToString();
    }
}
