using UnityEngine.UI;
using System.Text;
using UnityEngine;
using CharacterStat;
using CharacterStatModifier;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] private Text stateNameText;
    [SerializeField] private Text statModifiersText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(Stat stat,string statName)
    {
        stateNameText.text = GetStatValuesText(stat,statName);
        statModifiersText.text = GetStatModifiers(stat);

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private string GetStatValuesText(Stat stat,string statName)
    {
        sb.Length = 0;
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        if (stat.Value != stat.BaseValue)
        {
            sb.Append(" ( ");
            sb.Append(stat.BaseValue);

            if (stat.Value > stat.BaseValue)
            {
                sb.Append("+");
            }

            sb.Append(System.Math.Round(stat.Value - stat.BaseValue,4));
            sb.Append(") ");
        }

        return sb.ToString();
    }

    private string GetStatModifiers(Stat stat)
    {
        sb.Length = 0;

        foreach(StatModifier statModifier in stat.StatModifiers)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (statModifier.Value > 0)
            {
                sb.Append("+");
            }

            if(statModifier.Type == StatModifier.StatModType.Flat)
            {
                sb.Append(statModifier.Value);
            }
            else
            {
                sb.Append(statModifier.Value * 100);
                sb.Append("%");
            }

            EquipableItem item = statModifier.Source as EquipableItem;

            if (item != null)
            {
                sb.Append(" ");
                sb.Append(item.ItemName);
            }
            else
            {
                Debug.Log("Modifier is not an EI");
            }
        }

        return sb.ToString();
    }
 
}
