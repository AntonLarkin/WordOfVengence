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
    public EquipmentType EqipmentType;

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

}

