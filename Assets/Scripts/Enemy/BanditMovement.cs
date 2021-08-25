using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMovement : MonoBehaviour
{
    private enum State
    {
        Idle,
        Chase,
        Attack,
        Return,
        Die
    }

    [SerializeField] private float noticeRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    [SerializeField] private Player player;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = player.transform;
    }

    private void Update()
    {
        CheckState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, noticeRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }

    private void CheckState()
    {
        var playerPosition = playerTransform.position;
        var distance = Vector3.Distance(transform.position, playerPosition);

        if (distance < noticeRadius&&distance>attackRadius)
        {
            Debug.Log("notice");
        }
        else if (distance < attackRadius)
        {
            Debug.Log("Attack");
        }
        else if (distance > noticeRadius && distance < chaseRadius)
        {
            Debug.Log("Chase");
        }

    }
}
