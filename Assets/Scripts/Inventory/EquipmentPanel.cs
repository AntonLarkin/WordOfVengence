using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] private Transform equipmentSlotsParent;
    [SerializeField] private EquipmentSlot[] equipmentSlots;


    public EquipmentSlot[] EquipmentSlots => equipmentSlots;

    public event Action<ItemSlot> OnItemRightClickEvent;
    public event Action<ItemSlot> OnItemPointerEnterEvent;
    public event Action<ItemSlot> OnItemPointerExitEvent;
    public event Action<ItemSlot> OnItemBeginDragEvent;
    public event Action<ItemSlot> OnItemEndDragEvent;
    public event Action<ItemSlot> OnItemDragEvent;
    public event Action<ItemSlot> OnItemDropEvent;

    private void OnEnable()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnRightClickEvent += InvokeOnItemRightClickEvent;
            equipmentSlots[i].OnPointerEnterEvent += InvokeOnItemPointerEnterEvent;
            equipmentSlots[i].OnPointerExitEvent += InvokeOnItemPointerExitEvent;
            equipmentSlots[i].OnBeginDragEvent += InvokeOnItemBeginDragEvent;
            equipmentSlots[i].OnEndDragEvent += InvokeOnItemEndDragEvent;
            equipmentSlots[i].OnDragEvent += InvokeOnItemDragEvent;
            equipmentSlots[i].OnDropEvent += InvokeOnItemDropEvent;
        }
    }

    private void InvokeOnItemRightClickEvent(ItemSlot itemSlot)
    {
        OnItemRightClickEvent?.Invoke(itemSlot);
    }

    private void InvokeOnItemPointerEnterEvent(ItemSlot itemSlot)
    {
        OnItemPointerEnterEvent?.Invoke(itemSlot);
    }
    private void InvokeOnItemPointerExitEvent(ItemSlot itemSlot)
    {
        OnItemPointerExitEvent?.Invoke(itemSlot);
    }
    private void InvokeOnItemBeginDragEvent(ItemSlot itemSlot)
    {
        OnItemBeginDragEvent?.Invoke(itemSlot);
    }
    private void InvokeOnItemEndDragEvent(ItemSlot itemSlot)
    {
        OnItemEndDragEvent?.Invoke(itemSlot);
    }
    private void InvokeOnItemDragEvent(ItemSlot itemSlot)
    {
        OnItemDragEvent?.Invoke(itemSlot);
    }
    private void InvokeOnItemDropEvent(ItemSlot itemSlot)
    {
        OnItemDropEvent?.Invoke(itemSlot);
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
