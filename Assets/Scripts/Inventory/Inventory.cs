using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private ItemSlot[] itemSlots;

    private bool IsFull;

    public event Action<Item> OnItemRightClickEvent;

    private void OnEnable()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnRightClickEvent += InvokeOnItemRightClickEvent;
        }
    }
    private void Start()
    {
        UpdateUIInventory();
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }
    }

    private void InvokeOnItemRightClickEvent(Item item)
    {
        OnItemRightClickEvent?.Invoke(item);
    }
    private void UpdateUIInventory()
    {
        int i = 0;

        for (;i<items.Count && i< itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }

        for(; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }
    public bool IsAbleToRemoveItem(Item item)
    {
        if (items.Remove(item))
        {
            UpdateUIInventory();
            return true;
        }
        return false;
    } 

    public bool IsInventoryFull()
    {
        return items.Count >= itemSlots.Length;
    }

    public bool IsAbleToAddItem(Item item)
    {
        if (IsFull)
        {
            return false;
        }

        items.Add(item);
        UpdateUIInventory();
        return true;
    }

}
