using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private QuestGiver questGiver;

    public Text textDisplay;
    public string[] sentences;
    public string[] complitingQuestSentences;

    private int index;
    private bool isDialogfinished;
    private const float timeDelay = 0.015f;

    public int Index => index;
    public bool IsDialogFinished => isDialogfinished;

    private void Start()
    {
        questGiver = GetComponent<QuestGiver>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && textDisplay.enabled)
        {
            if (QuestManager.questManager.CheckAvailableQuests(questGiver))
            {
                TurnOnNextReplic(sentences);
            }
            else if(QuestManager.questManager.CheckCompletedQuests(questGiver))
            {
                TurnOnNextReplic(complitingQuestSentences);
            }
        }
    }

    public void StartDialogue(string[] sentences)
    {
        isDialogfinished = false;
        StartCoroutine(OnType(sentences));
    }

    public void StartingQuestDialog()
    {
        ClearText();
        index = 0;
        StartDialogue(sentences);
    }

    public void CompletingQuestDialog()
    {
        ClearText();
        index = 0;
        StartDialogue(complitingQuestSentences);
    }

    public void TurnOnNextReplic(string[] sentences)
    {
        if (index < sentences.Length-1)
        {
            index++;
            ClearText();
            StartDialogue(sentences);
        }
        else
        {
            isDialogfinished = true;
        }
    }


    public void ClearText()
    {
        textDisplay.text ="";
    }

    IEnumerator OnType(string[] sentencesVariant)
    {
        foreach (char letter in sentencesVariant[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(timeDelay);
        }
    }

}
