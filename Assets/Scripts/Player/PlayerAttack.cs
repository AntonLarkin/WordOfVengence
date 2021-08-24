using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTriggerName;
    [SerializeField] private string isAttackingBoolName;
    private CameraRaycast cameraRaycast;

    [Header("NavMeshAgent")]
    private NavMeshAgent navMeshAgent;
    private RaycastHit destinationInfo;
    private const float attackDistance = 3f;

    private bool isHeadingToAttacking;

    private void Awake()
    {
        cameraRaycast = FindObjectOfType<CameraRaycast>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        cameraRaycast.OnPlayerAttack += CameraRaycast_OnPlayerAttack;
    }

    private void OnDisable()
    {
        cameraRaycast.OnPlayerAttack -= CameraRaycast_OnPlayerAttack;
    }

    private void Update()
    {
        if (isHeadingToAttacking)
        {
            animator.SetBool(isAttackingBoolName, true);

            if (Vector3.Distance(transform.position, destinationInfo.point) <= attackDistance)
            {
                AttackPlayer();
                isHeadingToAttacking = false;
                animator.SetBool(isAttackingBoolName, false);
            }
        }
    }

    private void AttackPlayer()
    {
        animator.SetTrigger(attackTriggerName);
    }

    private void CameraRaycast_OnPlayerAttack(Vector3 endPosition)
    {
        destinationInfo.point = endPosition;

        isHeadingToAttacking = true;
        navMeshAgent.SetDestination(destinationInfo.point);

    }


}
