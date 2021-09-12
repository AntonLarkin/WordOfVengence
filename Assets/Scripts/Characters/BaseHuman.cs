using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHuman : MonoBehaviour
{
    [Header("BaseCharacteristics")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxStamina;

    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;

    public float CurrentHealth { get; private set; }
    public float CurrentStamina { get; private set; }

    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        CurrentStamina = maxStamina;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
        {
            return;
        }
        CurrentHealth -= damage;
    }

    protected abstract void Die();

}
