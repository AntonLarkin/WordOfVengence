using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string isWalkingBoolName;
    [SerializeField] private string isRunningBoolName;

    [Header("NavMeshAgent")]
    [SerializeField] private LayerMask accesableArea;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float walkingSpeed;
    private NavMeshAgent navMeshAgent;
    private RaycastHit destinationInfo;
    private const float minDistance = 0.25f;

    [Header("Double click cheker")]
    private const float timeBetweenClicks = 0.2f;
    private float firstClickTime = 0f;
    private bool isCoroutineDenied;
    private int clickCounter;

    private bool isMoving;

    private void Awake()
    {
        animator = FindObjectOfType<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        clickCounter = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
            FindPath();
        }

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

    private void FindPath()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var moveDirection = ray.direction.normalized;

        if (Physics.Raycast(ray, out destinationInfo, Mathf.Infinity, accesableArea))
        {
            navMeshAgent.SetDestination(destinationInfo.point);
        }

    }

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
        }

        if (isMoving)
        {
            animator.SetBool(isWalkingBoolName, true);
        }
        else if(!isMoving)
        {
            animator.SetBool(isWalkingBoolName, false);
            animator.SetBool(isRunningBoolName, false);
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

}
