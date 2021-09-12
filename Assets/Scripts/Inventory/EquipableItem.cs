using System.Collections;
using System.Collections.Generic;
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
}
