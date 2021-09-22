using CharacterStat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Stat Strength;
    public Stat Agility;
    public Stat Vitality;

    [SerializeField] private Player player;

    [SerializeField] private Inventory inventory;
    [SerializeField] private EquipmentPanel equipmentPanel;
    [SerializeField] private StatPanel statPanel;
    [SerializeField] private ItemTooltip itemTooltip;
    [SerializeField] private StatTooltip statTooltip;
    [SerializeField] private Image draggableItem;

    private ItemSlot draggedSlot;
    private bool isItemConsumed;

    private void OnValidate()
    {
        if (itemTooltip == null)
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }

        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    private void Awake()
    {
        statPanel.SetStats(Strength, Agility, Vitality);
        
        statPanel.UpdateStatValues();
    }

    private void OnEnable()
    {
        //RightClick
        inventory.OnItemRightClickEvent += InventoryRightClick;
        equipmentPanel.OnItemRightClickEvent += EquipmentPanelRightClick;
        //Pointer Enter
        inventory.OnItemPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnItemPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnItemPointerExitEvent += HideTooltip;
        equipmentPanel.OnItemPointerExitEvent += HideTooltip;
        //Begin Drug
        inventory.OnItemBeginDragEvent += BeginDrug;
        equipmentPanel.OnItemBeginDragEvent += BeginDrug;
        //End Drug
        inventory.OnItemEndDragEvent += EndDrug;
        equipmentPanel.OnItemEndDragEvent += EndDrug;
        //Drag
        inventory.OnItemDragEvent += Drag;
        equipmentPanel.OnItemDragEvent += Drag;
        //Drop
        inventory.OnItemDropEvent += Drop;
        equipmentPanel.OnItemDropEvent += Drop;

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
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
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
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.IsAbleToAddItem(item);
        }
    }

    private void InventoryRightClick(ItemSlot itemSlot)
    {
        if(itemSlot.Item is EquipableItem)
        {
            Equip((EquipableItem)itemSlot.Item);
        }
        else if(itemSlot.Item is UsableItem && !isItemConsumed)
        {
            Debug.Log(itemSlot.Amount);
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            usableItem.Use(player);

            isItemConsumed = true;
            if (usableItem.IsConsumable)
            {
                if (itemSlot.Amount > 1)
                {
                    itemSlot.Amount--;
                }
                else
                {
                    inventory.IsAbleToRemoveItem(usableItem);
                    usableItem.Destroy();
                }
            }

            StartCoroutine(OnReload());
        }

    }
    private void EquipmentPanelRightClick(ItemSlot itemSlot)
    {
        if (itemSlot.Item is EquipableItem)
        {
            Unequip((EquipableItem)itemSlot.Item);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }
    private void Drop(ItemSlot itemSlot)
    {
        if (draggedSlot == null)
        {
            return;
        }

        if (itemSlot.CanRecieveItem(draggedSlot.Item) && draggedSlot.CanRecieveItem(itemSlot.Item))
        {
            EquipableItem dragItem = draggedSlot.Item as EquipableItem;
            EquipableItem dropItem = itemSlot.Item as EquipableItem;


            if(draggedSlot is EquipmentSlot)
            {
                if (dragItem != null) dragItem.Unequip(this);
                if (dropItem != null) dropItem.Equip(this);
            }
            if(itemSlot is EquipmentSlot)
            {
                if (dragItem != null) dragItem.Equip(this);
                if (dropItem != null) dropItem.Unequip(this);
            }
            statPanel.UpdateStatValues();

            Item draggedItem = draggedSlot.Item;
            int draggedItemAmount = draggedSlot.Amount;

            draggedSlot.Item = itemSlot.Item;
            draggedSlot.Amount = itemSlot.Amount;

            itemSlot.Item = draggedItem;
            itemSlot.Amount = draggedItemAmount;
        }
    }

    private void Drag(ItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }
    }

    private void EndDrug(ItemSlot itemSlot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    private void BeginDrug(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private IEnumerator OnReload()
    {
        yield return new WaitForEndOfFrame();

        isItemConsumed = false;
    }
}
