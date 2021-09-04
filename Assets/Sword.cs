using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float damageValue;
    [SerializeField] private float staminaCost;

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
            other.GetComponent<BaseBandit>().TakeDamage(damageValue);
        }
    }


}
