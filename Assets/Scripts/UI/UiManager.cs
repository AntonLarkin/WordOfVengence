using System;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject conditionView;
    [SerializeField] private GameObject inventoryView;

    private bool isInventoryViewActive;

    public static event Action OnOpenInventory;
    public static event Action OnClosedInventory;

    private void Start()
    {
        inventoryView.SetActive (false);
        isInventoryViewActive = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryView(isInventoryViewActive);
        }
    }

    private void ToggleInventoryView(bool isActive)
    {
        inventoryView.SetActive(isActive);
        isInventoryViewActive = !isInventoryViewActive;
        if (isActive)
        {
            OnOpenInventory?.Invoke();
        }
        else
        {
            OnClosedInventory?.Invoke();
        }
    }

}
