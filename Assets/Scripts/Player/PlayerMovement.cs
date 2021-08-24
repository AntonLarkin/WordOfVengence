using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CameraRaycast cameraRaycast;

    [Header("Animator")]
    private Animator animator;
    [SerializeField] private string isWalkingBoolName;
    [SerializeField] private string isRunningBoolName;
    [SerializeField] private string isAttackingBoolName;

    [Header("NavMeshAgent")]
    [SerializeField] private LayerMask accesableArea;
    [SerializeField] private LayerMask unaccesableArea;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float walkingSpeed;
    private NavMeshAgent navMeshAgent;
    private RaycastHit destinationInfo;
    private float minDistance = 0.25f;

    [Header("Double click cheker")]
    private const float timeBetweenClicks = 0.2f;
    private float firstClickTime = 0f;
    private bool isCoroutineDenied;
    private int clickCounter;

    private bool isMoving;
    private bool isAttacking;

    private void Awake()
    {
        cameraRaycast = FindObjectOfType<CameraRaycast>();
    }

    private void OnEnable()
    {
        cameraRaycast.OnPlayerMove += CameraRaycast_OnPlayerMove;
        cameraRaycast.OnPlayerStop += CameraRaycast_OnPlayerStop;
        cameraRaycast.OnPlayerAttack += CameraRaycast_OnPlayerAttack;
    }

    private void OnDisable()
    {
        cameraRaycast.OnPlayerMove -= CameraRaycast_OnPlayerMove;
        cameraRaycast.OnPlayerStop -= CameraRaycast_OnPlayerStop;
        cameraRaycast.OnPlayerAttack -= CameraRaycast_OnPlayerAttack;
    }

    private void Start()
    {
        clickCounter = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            clickCounter++;
        }

        if(clickCounter==1 && !isCoroutineDenied)
        {
            CheckForSecondClick();
        }

        CheckForPlayerMovement();
    }

    /*private void FindPath()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var moveDirection = ray.direction.normalized;

        if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, unaccesableArea))
        {
            isMoving = false;
            return;
        }
        else if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, accesableArea))
        {
            isMoving = true;
            navMeshAgent.SetDestination(destinationInfo.point);
        }
    }*/

    private void CheckForSecondClick()
    {
        firstClickTime = Time.time;
        StartCoroutine(OnDoubleClick());
    }

    private void CheckForPlayerMovement()
    {
        if (Vector3.Distance(transform.position, destinationInfo.point) <= minDistance)
        {
            isMoving = false;
            isAttacking = false;
        }

        if (isMoving)
        {
            animator.SetBool(isWalkingBoolName, true);
        }
        else if (isAttacking)
        {
            animator.SetBool(isAttackingBoolName, true);
        }
        else if(!isMoving||!isAttacking)
        {
            animator.SetBool(isWalkingBoolName, false);
            animator.SetBool(isRunningBoolName, false);
            animator.SetBool(isAttackingBoolName, false);
            navMeshAgent.speed = walkingSpeed;
        }
    }

    private IEnumerator OnDoubleClick()
    {
        isCoroutineDenied = true;
        while (Time.time < firstClickTime + timeBetweenClicks)
        {
            if (clickCounter == 2)
            {
                animator.SetBool(isRunningBoolName, true);
                navMeshAgent.speed = runningSpeed;
            }

            yield return new WaitForEndOfFrame();
        }

        clickCounter = 0;
        firstClickTime = 0f;
        isCoroutineDenied = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, destinationInfo.point);
        Gizmos.DrawSphere(destinationInfo.point, 0.1f);
    }

    private void CameraRaycast_OnPlayerMove(Vector3 endPosition)
    {
        destinationInfo.point = endPosition;

        isMoving = true;
        navMeshAgent.SetDestination(destinationInfo.point);
    }

    private void CameraRaycast_OnPlayerAttack(Vector3 endPosition)
    {
        destinationInfo.point = endPosition;

        
        navMeshAgent.stoppingDistance = minDistance-1f;
        isAttacking = true;
        navMeshAgent.SetDestination(destinationInfo.point);

    }

    private void CameraRaycast_OnPlayerStop()
    {
        isMoving = false;
    }

}
