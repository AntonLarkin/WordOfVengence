using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damageValue;

    [SerializeField] private Player player;
    [SerializeField] private float destructionTime;

    [SerializeField] private Rigidbody rb;
    private Vector3 direction;

    private void Awake()
    {
        gameObject.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            other.GetComponent<Player>().TakeDamage(damageValue);
            Destroy(gameObject, destructionTime);
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * Time.deltaTime *speed, Space.Self);
    }

}
