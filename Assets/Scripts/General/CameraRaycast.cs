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

    private bool isInventaryOpened;
    private bool isQuestViewOpened;

    public event Action<Vector3> OnPlayerMove;
    public event Action OnPlayerStop;
    public event Action<Vector3> OnPlayerInteract;
    public event Action<Vector3> OnPlayerAttack;

    private void OnEnable()
    {
        UiManager.OnOpenInventory += UiManager_OnOpenInventory;
        UiManager.OnClosedInventory += UiManager_OnClosedInventory;
        UiManager.OnOpenQuestView += UiManager_OnOpenQuestView;
        UiManager.OnClosedQuestView += UiManager_OnClosedQuestView;
    }
    private void OnDisable()
    {
        UiManager.OnOpenInventory -= UiManager_OnOpenInventory;
        UiManager.OnClosedInventory -= UiManager_OnClosedInventory;
        UiManager.OnOpenQuestView -= UiManager_OnOpenQuestView;
        UiManager.OnClosedQuestView -= UiManager_OnClosedQuestView;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&!isQuestViewOpened&&!isInventaryOpened)
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
        isInventaryOpened = true;
    }
    private void UiManager_OnClosedInventory()
    {
        isInventaryOpened = false;
    }

    private void UiManager_OnOpenQuestView()
    {
        isQuestViewOpened = true;
    }

    private void UiManager_OnClosedQuestView()
    {
        isQuestViewOpened = false;
    }

}
