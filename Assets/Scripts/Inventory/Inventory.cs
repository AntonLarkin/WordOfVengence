using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> startingItems;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private ItemSlot[] itemSlots;

    private bool IsFull;

    public event Action<ItemSlot> OnItemRightClickEvent;
    public event Action<ItemSlot> OnItemPointerEnterEvent;
    public event Action<ItemSlot> OnItemPointerExitEvent;
    public event Action<ItemSlot> OnItemBeginDragEvent;
    public event Action<ItemSlot> OnItemEndDragEvent;
    public event Action<ItemSlot> OnItemDragEvent;
    public event Action<ItemSlot> OnItemDropEvent;

    private void OnEnable()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnRightClickEvent += InvokeOnItemRightClickEvent;
            itemSlots[i].OnPointerEnterEvent += InvokeOnItemPointerEnterEvent;
            itemSlots[i].OnPointerExitEvent += InvokeOnItemPointerExitEvent;
            itemSlots[i].OnBeginDragEvent += InvokeOnItemBeginDragEvent;
            itemSlots[i].OnEndDragEvent += InvokeOnItemEndDragEvent;
            itemSlots[i].OnDragEvent += InvokeOnItemDragEvent;
            itemSlots[i].OnDropEvent += InvokeOnItemDropEvent;
        }
    }
    private void Start()
    {
        SetStartingItems();
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
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

    private void SetStartingItems()
    {
        int i = 0;

        for (;i<startingItems.Count && i< itemSlots.Length; i++)
        {
            itemSlots[i].Item = startingItems[i];
        }

        for(; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
    }
    public bool IsAbleToRemoveItem(Item item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                itemSlots[i].Item = null;
                return true;
            }
        }
        return false;
    } 

    public bool IsInventoryFull()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAbleToAddItem(Item item)
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                return true;
            }
        }
        return false;
    }

}
