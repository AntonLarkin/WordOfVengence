using CharacterStat;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Stat Strength;
    public Stat Agility;
    public Stat Vitality;

    [SerializeField] private Inventory inventory;
    [SerializeField] private EquipmentPanel equipmentPanel;
    [SerializeField] private StatPanel statPanel;

    private void Awake()
    {
        statPanel.SetStats(Strength, Agility, Vitality);
        
        statPanel.UpdateStatValues();
    }

    private void OnEnable()
    {
        inventory.OnItemRightClickEvent += EquipFromInvemtory;
        equipmentPanel.OnItemRightClickEvent += UnequipFromEquipPanel;
    }

    private void Update()
    {

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
