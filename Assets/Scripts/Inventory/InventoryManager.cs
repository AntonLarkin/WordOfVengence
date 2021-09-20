using CharacterStat;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Stat Strength;
    public Stat Agility;
    public Stat Vitality;

    [SerializeField] private Inventory inventory;
    [SerializeField] private EquipmentPanel equipmentPanel;
    [SerializeField] private StatPanel statPanel;
    [SerializeField] private ItemTooltip itemTooltip;
    [SerializeField] private StatTooltip statTooltip;
    [SerializeField] private Image draggableItem;

    private ItemSlot draggedSlot;

    private void OnValidate()
    {
        if (itemTooltip == null)
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
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
        inventory.OnItemRightClickEvent += Equip;
        equipmentPanel.OnItemRightClickEvent += Unequip;
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

    private void Equip(ItemSlot itemSlot)
    {
        EquipableItem equipableItem = itemSlot.Item as EquipableItem;
        if (equipableItem != null)
        {
            Equip(equipableItem);
        }
    }
    private void Unequip(ItemSlot itemSlot)
    {
        EquipableItem equipableItem = itemSlot.Item as EquipableItem;
        if (equipableItem != null)
        {
            Unequip(equipableItem);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        EquipableItem equipableItem = itemSlot.Item as EquipableItem;
        if (equipableItem != null)
        {
            itemTooltip.ShowTooltip(equipableItem);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }
    private void Drop(ItemSlot itemSlot)
    {
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
            draggedSlot.Item = itemSlot.Item;
            itemSlot.Item = draggedItem;

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
}
