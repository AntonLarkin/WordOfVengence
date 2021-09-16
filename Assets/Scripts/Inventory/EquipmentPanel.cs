using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] private Transform equipmentSlotsParent;
    [SerializeField] private EquipmentSlot[] equipmentSlots;

    public event Action<Item> OnItemRightClickEvent;

    private void OnEnable()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnRightClickEvent += InvokeOnItemRightClickEvent;
        }
    }

    private void  InvokeOnItemRightClickEvent(Item item)
    {
        OnItemRightClickEvent?.Invoke(item);
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool IsAbleToAddItem(EquipableItem equipableItem, out EquipableItem alreadyUsedItem)
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].EquipmentType == equipableItem.EqipmentType)
            {
                alreadyUsedItem = (EquipableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = equipableItem;
                return true;
            }
        }
        alreadyUsedItem = null;
        return false;
    }

    public bool IsAbleToRemoveItem(EquipableItem equipableItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == equipableItem)
            {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}
