using CharacterStat;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    [SerializeField] private StatDisplay[] statDisplays;
    [SerializeField] private string[] statNames;

    private Stat[] characterStats;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();

        UpdateStatNames();
    }

    public void SetStats(params Stat[] stats)
    {
        characterStats = stats;

        if (characterStats.Length > statDisplays.Length)
        {
            Debug.LogError("Not enough stats!");
            return;
        }

        for(int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(i < characterStats.Length);

            if (i < stats.Length)
            {
                statDisplays[i].Stat = stats[i];
            }
        }

        for (int i = 0; i < characterStats.Length; i++)
        {
            characterStats[i].SetDefaultValue(10);
        }
    }

    public void UpdateStatValues()
    {
        for(int i = 0; i < characterStats.Length; i++)
        {
            statDisplays[i].UpdateStatValue();
        }
    }

    public void UpdateStatNames()
    {
        for (int i = 0; i < statNames.Length; i++)
        {
            statDisplays[i].Name = statNames[i];
        }
    }
}
