using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text textDisplay;
    public string[] sentences;

    private int index;
    private bool isDialogfinished;
    private const float timeDelay = 0.02f;

    public int Index => index;
    public bool IsDialogFinished => isDialogfinished;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && textDisplay.enabled)
        {
            TurnOnNextReplic();
        }
    }

    public void StartDialogue()
    {
        isDialogfinished = false;
        StartCoroutine(OnType());
    }

    public void TurnOnNextReplic()
    {
        if (index < sentences.Length-1)
        {
            index++;
            ClearText();
            StartDialogue();
        }
        else
        {
            isDialogfinished = true;
            QuestManager.questManager.AcceptQuest(1);
        }
    }

    public void ClearText()
    {
        textDisplay.text ="";
    }

    IEnumerator OnType()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(timeDelay);
        }
    }

}
