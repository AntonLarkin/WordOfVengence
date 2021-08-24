using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Animator")]
    private Animator animator;
    [SerializeField] private string attackTriggerName;
    private CameraRaycast cameraRaycast;

    private void Awake()
    {
        cameraRaycast = FindObjectOfType<CameraRaycast>();
    }

    private void OnEnable()
    {
        cameraRaycast.OnPlayerAttack += CameraRaycast_OnPlayerAttack;
    }

    private void OnDisable()
    {
        cameraRaycast.OnPlayerAttack -= CameraRaycast_OnPlayerAttack;
    }

    private void Start()
    {
        animator.GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void Update()
    {
        
    }

    private void AttackPlayer()
    {
        animator.SetTrigger(attackTriggerName);
    }

    private void CameraRaycast_OnPlayerAttack(Vector3 endPosition)
    {
        
    }

}
