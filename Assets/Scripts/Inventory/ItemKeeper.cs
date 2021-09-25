using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private bool isEndless;

    public Item[] CollectItems()
    {
        if (items != null)
        {
            return items;
        }
        return null;
    }

    public void CleanItemKeeper()
    {
        if (!isEndless)
        {
            items = null;
        }
    } 
}
