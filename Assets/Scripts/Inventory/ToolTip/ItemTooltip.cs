using UnityEngine.UI;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemTypeText;
    [SerializeField] private Text itemDescriptionText;


    public void ShowTooltip(Item item)
    {
        itemNameText.text = item.ItemName;
        itemTypeText.text = item.GetItemType();
        itemDescriptionText.text = item.GetDescription();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

}
