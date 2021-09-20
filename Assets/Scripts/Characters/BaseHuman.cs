using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHuman : MonoBehaviour
{

    [SerializeField] protected int id;

    [Header("BaseCharacteristics")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxStamina;

    public int ID => id;
    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;

    public float CurrentHealth { get; protected set; }
    public float CurrentStamina { get; protected set; }

    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        CurrentStamina = maxStamina;
    }

    protected virtual void Update()
    {
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
        {
            return;
        }
        CurrentHealth -= damage;
    }

    public void TakeStaminaExhaustion(float exhaustionValue)
    {
        if (CurrentStamina <= 0)
        {
            return;
        }
        CurrentStamina = maxStamina - exhaustionValue;
    }

    protected abstract void Die();



}
