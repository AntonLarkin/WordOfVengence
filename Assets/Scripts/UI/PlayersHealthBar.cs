using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayersHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image staminaBarImage;

    [SerializeField] private Player player;

    private float maxHealth;
    private float maxStamina;
    private float currentHealth;
    private float currentStamina;

    private void Start()
    {
        
    }

    private void Update()
    {
        maxHealth = player.MaxHealth;
        maxStamina = player.MaxStamina;

        ChangeHealth();
        ChangeStamina();
    }

    private void ChangeStamina()
    {
        currentStamina = player.CurrentStamina;

        staminaBarImage.fillAmount = currentStamina / maxStamina;
    }

    private void ChangeHealth()
    {
        currentHealth = player.CurrentHealth;

        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

}
