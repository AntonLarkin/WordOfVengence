using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Item item;

    public Item GetItem()
    {
        return item;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            //QuestItemCollector();
        }
    }

    public void QuestItemCollector()
    {
        var qm = QuestManager.questManager;
        for(int i = 0; i < qm.currentQuests.Count; i++)
        {
            if(item == qm.currentQuests[i].questItem)
            {
                if(qm.currentQuests[i].questObjectiveCount < qm.currentQuests[i].questObjectiveRequirment)
                {
                    qm.AddQuestItem(qm.currentQuests[i].questId,1);
                }
                else
                {
                    qm.CompleteQuest(qm.currentQuests[i].questId);
                }
            }
        }
    }

}
