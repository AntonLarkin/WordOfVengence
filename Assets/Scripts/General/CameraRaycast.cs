using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask accesableArea;
    [SerializeField] private LayerMask unaccesableArea;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private LayerMask item;

    private RaycastHit destinationInfo;

    private bool isRaicastingActive = true;

    public event Action<Vector3> OnPlayerMove;
    public event Action OnPlayerStop;
    public event Action<Vector3> OnPlayerInteract;
    public event Action<Vector3> OnPlayerAttack;

    private void OnEnable()
    {
        UiManager.OnOpenInventory += UiManager_OnOpenInventory;
        UiManager.OnClosedInventory += UiManager_OnClosedInventory;
    }
    private void OnDisable()
    {
        UiManager.OnOpenInventory -= UiManager_OnOpenInventory;
        UiManager.OnClosedInventory -= UiManager_OnClosedInventory;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&isRaicastingActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, unaccesableArea))
            {
                OnPlayerStop?.Invoke();
            }

            else if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, enemy))
            {
                
                OnPlayerAttack?.Invoke(destinationInfo.point);
            }
            else if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, item))
            {
                OnPlayerInteract?.Invoke(destinationInfo.point);
            }
            else if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, accesableArea))
            {
                OnPlayerMove?.Invoke(destinationInfo.point);
            }


        }

    }

    private void UiManager_OnOpenInventory()
    {
        isRaicastingActive = false;
    }
    private void UiManager_OnClosedInventory()
    {
        isRaicastingActive = true;
    }

}
