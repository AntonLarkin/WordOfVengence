using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CharacterStat;

public class StatDisplay : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StatTooltip tooltip;

    [SerializeField] Text nameText;
    [SerializeField] Text valueText;

    private Stat _stat;
    private string _name;
    public Stat Stat
    {
        get
        {
            return _stat;
        }
        set
        {
            _stat = value;
            UpdateStatValue();
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            nameText.text = _name;
        }
    }

    private void OnValidate()
    {
        if (tooltip == null)
        {
            tooltip = FindObjectOfType<StatTooltip>();
        }
        
        Text[] texts = GetComponentsInChildren<Text>();
        nameText = texts[0];
        valueText = texts[1];
    }

    private void Update()
    {

    }

    public void UpdateStatValue()
    {
        valueText.text = _stat.Value.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(Stat,Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
