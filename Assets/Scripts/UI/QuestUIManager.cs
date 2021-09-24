using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager questUiManager;

    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject questLogPanel;

    public List<BaseQuest> availableQuests = new List<BaseQuest>();
    public List<BaseQuest> activeQuests = new List<BaseQuest>();

    public GameObject qButton;
    public GameObject qLogButton;
    private List<GameObject> qButtons = new List<GameObject>();

    private GameObject acceptButton;

    public Transform qButtonSpacerAvailable;
    public Transform qButtonSpacerRunning;
    public Transform qLogButtonSpacer;

    public bool isQuestAvailable;
    public bool isQuestRunning;
    public bool isQuestPanelActive;
    private bool isQuestLogPanelActive;

    public Text questTitle;
    public Text questDesription;
    public Text questSummery;

    public Text questLogTitle;
    public Text questLogDesription;
    public Text questLogSummery;

    private QuestGiver currentQuestGiver;

    private void Awake()
    {
        if (questUiManager == null)
        {
            questUiManager = this;
        }
        else if (questUiManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        HideQuestPanel();
    }

    public void CheckQuests(QuestGiver questGiver)
    {
        currentQuestGiver = questGiver;
        QuestManager.questManager.QuestRequest(currentQuestGiver);
        if((isQuestRunning || isQuestAvailable) && !isQuestPanelActive)
        {
            ClearQuestButtons();
            ShowQuestPanel();
        }
        else
        {
            Debug.Log("No quest available");
        }
    }

    public void ShowQuestPanel()
    {
        isQuestPanelActive = true;
        FillQuestButtons();
        questPanel.SetActive(isQuestPanelActive);
    }

    public void HideQuestPanel()
    {
        isQuestPanelActive = false;
        isQuestAvailable = false;
        isQuestRunning = false;

        questTitle.text = "";
        questDesription.text = "";
        questSummery.text = "";

        availableQuests.Clear();
        activeQuests.Clear();

        for(int i = 0; i < qButtons.Count; i++)
        {
            Destroy(qButtons[i]);
        }
        qButtons.Clear();

        questPanel.SetActive(isQuestPanelActive);
    }

    public void MoveAcceptedQuestToActive(int questID)
    {
        for(int i=0;i< QuestManager.questManager.currentQuests.Count; i++)
        {
            if (QuestManager.questManager.currentQuests[i].questId == questID &&
                QuestManager.questManager.currentQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {
                activeQuests.Add(QuestManager.questManager.currentQuests[i]);
                availableQuests.Remove(QuestManager.questManager.currentQuests[i]);
                ClearQuestButtons();
            }

        }
        FillQuestButtons();
    }

    public void CompleteQuestInQuestView(int questID)
    {
        for (int i = 0; i < QuestManager.questManager.currentQuests.Count; i++)
        {
            if (QuestManager.questManager.currentQuests[i].questId == questID &&
                QuestManager.questManager.currentQuests[i].questProgression == BaseQuest.QuestProgression.Completed)
            {
                activeQuests.Remove(QuestManager.questManager.currentQuests[i]);
                ClearQuestButtons();
            }

        }

        FillQuestButtons();
    }

    private void CheckForRequirmentItems()      // just in case =)
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            if (activeQuests[i].questProgression == BaseQuest.QuestProgression.Accepted)
            {

                if(activeQuests[i].questObjectiveCount <= activeQuests[i].questObjectiveRequirment)
                {
                    questDesription.text = activeQuests[i].questDescription;
                    questSummery.text = activeQuests[i].questObjective + ":" + activeQuests[i].questObjectiveCount + "/" +
                        activeQuests[i].questObjectiveRequirment;
                }
            }
            if (activeQuests[i].questProgression == BaseQuest.QuestProgression.Completed)
            {
                questDesription.text = activeQuests[i].acomplishmentMessage;
                questSummery.text = activeQuests[i].questObjective + ":" + activeQuests[i].questObjectiveRequirment + "/" +
                    activeQuests[i].questObjectiveRequirment;
            }
        }
    }

    public void ClearQuestButtons()
    {
        questTitle.text = "";
        questDesription.text = "";
        questSummery.text = "";

        for (int i = 0; i < qButtons.Count; i++)
        {
            Destroy(qButtons[i]);
        }
    }

    private void FillQuestButtons()
    {

        foreach(BaseQuest availableQuest in availableQuests)
        {
            GameObject questButton = Instantiate(qButton);
            QButton qButtonScript = questButton.GetComponent<QButton>();
            qButtonScript.questID = availableQuest.questId;
            qButtonScript.questTitle.text = availableQuest.questTitle;

            questButton.transform.SetParent(qButtonSpacerAvailable, false);
            qButtons.Add(questButton);
        }

        foreach (BaseQuest activeQuest in activeQuests)
        {
            GameObject questButton = Instantiate(qButton);
            QButton qButtonScript = questButton.GetComponent<QButton>();
            qButtonScript.questID = activeQuest.questId;
            qButtonScript.questTitle.text = activeQuest.questTitle;

            questButton.transform.SetParent(qButtonSpacerRunning, false);
            qButtons.Add(questButton);
        }
    }

    public void ShowSelectedQuest(int questID)
    {
        for (int i = 0; i < availableQuests.Count; i++)
        {
            if(availableQuests[i].questId == questID)
            {
                questTitle.text = availableQuests[i].questTitle;
                if(availableQuests[i].questProgression == BaseQuest.QuestProgression.Available)
                {
                    questDesription.text = availableQuests[i].questDescription;
                    questSummery.text = availableQuests[i].questObjective + ":" + availableQuests[i].questObjectiveCount + "/" +
                        availableQuests[i].questObjectiveRequirment;
                }
            }
        }
        for(int i = 0; i < activeQuests.Count; i++)
        {
            if(activeQuests[i].questId == questID)
            {
                questTitle.text = activeQuests[i].questTitle;
                if (activeQuests[i].questProgression == BaseQuest.QuestProgression.Accepted) 
                {
                    questDesription.text = activeQuests[i].questDescription;
                    questSummery.text = activeQuests[i].questObjective + ":" + activeQuests[i].questObjectiveCount + "/" +
                        activeQuests[i].questObjectiveRequirment;
                }
                else if(activeQuests[i].questProgression == BaseQuest.QuestProgression.Completed)
                {
                    questDesription.text = activeQuests[i].acomplishmentMessage;
                    questSummery.text = activeQuests[i].questObjective + ":" + activeQuests[i].questObjectiveRequirment + "/" +
                        activeQuests[i].questObjectiveRequirment;
                }

            }
        }
    }

}
