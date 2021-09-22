using CharacterStatModifier;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chest,
    Pants,
    Boots,
    Weapon1,
    Weapon2
}

[CreateAssetMenu(fileName = "New Equipable Item", menuName = "Inventory/Equipable Item")]
public class EquipableItem : Item
{
    public int StrengthBonus;
    public int AgilityBonus;
    public int VitalityBonus;

    [Space]
    public float StrengthPercentBonus;
    public float AgilityPercentBonus;
    public float  VitalityPercentBonus;

    [Space]
    public EquipmentType EquipmentType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(InventoryManager character)
    {
        if (StrengthBonus != 0)
        {
            character.Strength.AddModifier(new StatModifier(StrengthBonus, StatModifier.StatModType.Flat, this));
        }
        if (AgilityBonus != 0)
        {
            character.Agility.AddModifier(new StatModifier(AgilityBonus, StatModifier.StatModType.Flat, this));
        }
        if (VitalityBonus != 0)
        {
            character.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModifier.StatModType.Flat, this));
        }

        if (StrengthPercentBonus != 0)
        {
            character.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModifier.StatModType.PercentMult, this));
        }
        if (AgilityPercentBonus != 0)
        {
            character.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModifier.StatModType.PercentMult, this));
        }
        if (VitalityPercentBonus != 0)
        {
            character.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModifier.StatModType.PercentMult, this));
        }
    }

    public void Unequip(InventoryManager character)
    {
        character.Strength.RemoveAllModifiersFromSource(this);
        character.Agility.RemoveAllModifiersFromSource(this);
        character.Vitality.RemoveAllModifiersFromSource(this);
    }

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        AddStat(StrengthBonus, "Strength");
        AddStat(AgilityBonus, "Agility");
        AddStat(VitalityBonus, "Vitality");

        AddStat(StrengthPercentBonus, "Strength", true);
        AddStat(AgilityPercentBonus, "Agility", true);
        AddStat(VitalityPercentBonus, "Vitality", true);
        return sb.ToString();
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
                sb.Append(value * 100);
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

