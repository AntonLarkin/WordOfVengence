using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStat;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damageValue;
    [SerializeField] private float staminaCost;
    [SerializeField] private InventoryManager inventoryManager;

    public float DamageValue => damageValue;
    public float StaminaCost => staminaCost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) )
        {
            other.GetComponent<Player>().TakeDamage(damageValue);
        }
        if (other.CompareTag(Tags.Enemy))
        {
            other.GetComponent<BaseBandit>().TakeDamage(GetDamageValue());
            gameObject.GetComponentInParent<BaseHuman>().TakeStaminaExhaustion(staminaCost);
        }
    }

    private float GetDamageValue()
    {
        Debug.Log(inventoryManager.Strength.GetFinalValue());
        return inventoryManager.Strength.GetFinalValue();
    }
}
