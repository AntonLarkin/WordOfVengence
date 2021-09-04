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
    private float currentHealt;
    private float currentStamina;

    private void Start()
    {
        maxHealth = player.MaxHealth;
        maxStamina = player.MaxStamina;
    }

    private void Update()
    {
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
        currentHealt = player.CurrentHealth;

        healthBarImage.fillAmount = currentHealt / maxHealth;
    }

}
