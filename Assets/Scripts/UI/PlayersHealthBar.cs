using UnityEngine;
using UnityEngine.UI;

public class PlayersHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float maxHealth;
    
    private float currentHealt;

    private void Start()
    {
        currentHealt = maxHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentHealt -= 10f;
        }
        ChangeHealth();
    }

    
    private void ChangeHealth()
    {
        healthBarImage.fillAmount = currentHealt / maxHealth;
    }

}
