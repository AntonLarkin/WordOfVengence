using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private Item item;
    [SerializeField] private Image image;

    public event Action<Item> OnRightClickEvent;
    public Item Item
    {
        get { return item; }
        set
        {
            item = value;
            if(item == null)
            {
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
    }

    private void Update()
    {
        Debug.Log(OnRightClickEvent != null);
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


}
