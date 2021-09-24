using System;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject conditionView;
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private QuestUIManager questsView;

    private bool isInventoryViewActive;
    private bool isQuestViewActive;

    public static event Action OnOpenInventory;
    public static event Action OnClosedInventory;
    public static event Action OnOpenQuestView;
    public static event Action OnClosedQuestView;

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

        if (Input.GetKeyDown(KeyCode.J))
        {
            isQuestViewActive = questsView.isQuestPanelActive;
            ToggleQuestView(isQuestViewActive);
        }

        if (questsView.isQuestPanelActive)
        {
            OnOpenQuestView?.Invoke();
        }
        else
        {
            OnClosedQuestView?.Invoke();
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

    private void ToggleQuestView(bool isQuestViewActive)
    {
        this.isQuestViewActive = !isQuestViewActive;

        if (!isQuestViewActive)
        {
            questsView.ShowQuestPanel();
        }
        else
        {
            questsView.isQuestPanelActive = false;
            QuestUIManager.questUiManager.ClearQuestButtons();
            questsView.gameObject.SetActive(questsView.isQuestPanelActive);
        }
    }

}
