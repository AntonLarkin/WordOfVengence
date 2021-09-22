using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItemEffect : ScriptableObject
{
    public abstract void ExecuteEffect(UsableItem usableItem, Player player);
    public abstract string GetDescription();
}
