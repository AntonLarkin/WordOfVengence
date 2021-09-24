using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QButton : MonoBehaviour
{
    public int questID;
    public Text questTitle;

    private GameObject acceptButton;
    private GameObject completeButton;

    private QButton acceptButtonScript;
    private QButton completeButtonScript;

    private void Start()
    {
        acceptButton = GameObject.Find("Canvas").transform.FindChild("QuestView").transform.FindChild("Description").transform.FindChild("AcceptButton").gameObject;
        acceptButtonScript = acceptButton.GetComponent<QButton>();

        completeButton = GameObject.Find("Canvas").transform.FindChild("QuestView").transform.FindChild("Description").transform.FindChild("CompleteButton").gameObject;
        completeButtonScript = completeButton.GetComponent<QButton>();

        acceptButton.SetActive(false);
        completeButton.SetActive(false);
    }

    public void ShowInfo()
    {
        QuestUIManager.questUiManager.ShowSelectedQuest(questID);

        if (QuestManager.questManager.RequestAvailableQuest(questID))
        {
            acceptButton.SetActive(true);
            completeButton.SetActive(false);
            acceptButtonScript.questID = questID;
            completeButtonScript.questID = questID;
        }
        else if (QuestManager.questManager.RequestCompletedQuest(questID))
        {
            completeButton.SetActive(true);
            acceptButton.SetActive(false);
            acceptButtonScript.questID = questID;
            completeButtonScript.questID = questID;
        }
        else 
        {
            acceptButton.SetActive(false);
            completeButton.SetActive(false);
        }
    }

    public void AcceptButtonQuest()
    {
        QuestManager.questManager.AcceptQuest(questID);
        QuestUIManager.questUiManager.MoveAcceptedQuestToActive(questID);
    }

    public void CompleteButtonQuest()
    {
        QuestUIManager.questUiManager.CompleteQuestInQuestView(questID);
        QuestManager.questManager.DoneQuest(questID);
        completeButton.SetActive(false);

    }

    public void Closepanel()
    {
        QuestUIManager.questUiManager.HideQuestPanel();
    }

}
