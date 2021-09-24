using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    
    [SerializeField] private GameObject triggerText;
    [SerializeField] private GameObject DialogObject;

    public List<int> availableQuestIDs = new List<int>();
    public List<int> receivableQuestIDs = new List<int>();

    public bool isQuestReadyToBeDone;

    [SerializeField] private Dialog dialog;

    private bool isTalking;
    private bool isQuestAccepted;

    private void Start()
    {
        dialog = GetComponent<Dialog>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && gameObject.CompareTag(Tags.Npc))
        {
            gameObject.GetComponent<Dialog>().enabled = true;
            if (!isTalking)
            {
                triggerText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (QuestManager.questManager.CheckAvailableQuests(this))
                    {
                        isTalking = true;
                        triggerText.SetActive(false);
                        DialogObject.SetActive(true);
                        dialog.StartingQuestDialog();
                    }
                    else if (QuestManager.questManager.CheckCompletedQuests(this))
                    {
                        isTalking = true;
                        triggerText.SetActive(false);
                        DialogObject.SetActive(true);
                        dialog.CompletingQuestDialog();
                    }
                }
            }
            if (dialog.IsDialogFinished)
            {
                if (isTalking)
                {
                    QuestUIManager.questUiManager.CheckQuests(this);

                }

                EnableDialogueWindow(false);
            }
            
        }
        else if (other.CompareTag(Tags.Player) && gameObject.CompareTag(Tags.TriggerPoint))
        {
            gameObject.GetComponent<Dialog>().enabled = true;
            if (!isTalking)
            {
                if (QuestManager.questManager.CheckAvailableQuests(this))
                {
                    isTalking = true;
                    DialogObject.SetActive(true);
                    dialog.StartingQuestDialog();
                }
                else if (QuestManager.questManager.CheckCompletedQuests(this))
                {
                    isTalking = true;
                    DialogObject.SetActive(true);
                    dialog.CompletingQuestDialog();
                }
            }
            if (dialog.IsDialogFinished)
            {
                if (isTalking)
                {
                    QuestUIManager.questUiManager.CheckQuests(this);
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                }

                EnableDialogueWindow(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnableDialogueWindow(false);
        gameObject.GetComponent<Dialog>().enabled = false;
    }

    private void EnableDialogueWindow(bool isDialogActive)
    {
        triggerText.SetActive(isDialogActive);
        isTalking = isDialogActive;
        DialogObject.SetActive(isDialogActive);
        dialog.ClearText();
    }
}
