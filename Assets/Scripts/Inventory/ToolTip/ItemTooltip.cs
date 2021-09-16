using UnityEngine.UI;
using System.Text;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemSlotText;
    [SerializeField] private Text itemStatsText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(EquipableItem item)
    {
        itemNameText.text = item.ItemName;
        itemSlotText.text = item.EqipmentType.ToString();

        sb.Length = 0;
        AddStat(item.StrengthBonus, "Strength");
        AddStat(item.AgilityBonus, "Agility");
        AddStat(item.VitalityBonus, "Vitality");

        AddStat(item.StrengthPercentBonus, "Strength", true);
        AddStat(item.AgilityPercentBonus, "Agility", true);
        AddStat(item.VitalityPercentBonus, "Vitality", true);

        itemStatsText.text = sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }
            if (value > 0)
            {
                sb.Append("+");
            }

            if (isPercent)
            {
                sb.Append(value*100);
                sb.Append("%  ");
            }
            else
            {
                sb.Append(value);
                sb.Append("  ");
            }

            sb.Append(statName);
        }
    }
}
