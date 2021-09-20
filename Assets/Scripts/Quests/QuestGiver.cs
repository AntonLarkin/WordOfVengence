using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject triggerText;
    [SerializeField] private GameObject DialogObject;

    private Dialog dialog;
    private bool isTalking;

    private void Start()
    {
        dialog = GetComponent<Dialog>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            if (!isTalking)
            {
                triggerText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isTalking = true;
                    triggerText.SetActive(false);
                    DialogObject.SetActive(true);
                    dialog.StartDialogue();
                }
            }
            if (dialog.IsDialogFinished)
            {
                triggerText.SetActive(false);
                isTalking = false;
                DialogObject.SetActive(false);
                dialog.ClearText();
                triggerText.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggerText.SetActive(false);
        isTalking = false;
        DialogObject.SetActive(false);
        dialog.ClearText();

    }

    private void EnableDialogueWindow(bool isDialogActive)
    {

    }
}
