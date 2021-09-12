using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private EquipmentPanel equipmentPanel;

    private void Awake()
    {
        inventory.OnItemRightClickEvent += EquipFromInvemtory;
        equipmentPanel.onItemRightClickEvent += UnequipFromEquipPanel;
    }

    public void Equip(EquipableItem item)
    {
        if (inventory.IsAbleToRemoveItem(item))
        {
            EquipableItem previousItem;
            if(equipmentPanel.IsAbleToAddItem(item,out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.IsAbleToAddItem(previousItem);
                }
            }
        }
        else
        {
            inventory.IsAbleToAddItem(item);
        }
    }

    public void Unequip(EquipableItem item)
    {
        if (!inventory.IsInventoryFull() && equipmentPanel.IsAbleToRemoveItem(item))
        {
            inventory.IsAbleToAddItem(item);
        }
    }

    private void EquipFromInvemtory(Item item)
    {
        if(item is EquipableItem)
        {
            Equip((EquipableItem)item);
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        if (item is EquipableItem)
        {
            Unequip((EquipableItem)item);
        }
    }
}
