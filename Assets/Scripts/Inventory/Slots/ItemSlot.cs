using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour , IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private Item item;
    [SerializeField] private Image image;
    [SerializeField] private ItemTooltip tooltip;

    public event Action<Item> OnRightClickEvent;
    public Item Item
    {
        get { return item; }
        set
        {
            item = value;
            if(item == null)
            {
                image.sprite = null; 
                image.enabled = false;
            }
            else
            {
                image.sprite = item.Icon;
                image.enabled = true;
            }
        }
    }
    protected virtual void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (tooltip == null)
        {
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item!=null && OnRightClickEvent != null)
            {
                OnRightClickEvent(Item);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Item is EquipableItem)
        {
            tooltip.ShowTooltip((EquipableItem)Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
