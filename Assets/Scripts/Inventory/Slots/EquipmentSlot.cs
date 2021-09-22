using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType EquipmentType;
    public int EquipmentSlotID;

    protected override void OnValidate()
    {
        base.OnValidate();

        gameObject.name = EquipmentType.ToString() + " Slot";
    }

    public override bool CanRecieveItem(Item item)
    {
        if (item == null)
        {
            return true;
        }

        EquipableItem equipableItem = item as EquipableItem;
        return equipableItem != null && equipableItem.EquipmentType == EquipmentType;
    }
}
