using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string isWalkingBoolName;
    [SerializeField] private string isRuningBoolName;

    [Header ("Movement and rotation")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    bool isMoving;

    [Header("Gravity")]
    [SerializeField] private float gravityScale;

    [Header("NavMeshAgent")]
    [SerializeField] private LayerMask accesableArea;
    private NavMeshAgent navMeshAgent;
    private RaycastHit destinationInfo;
    private const float minDistance = 0.25f;

    private void Awake()
    {
        animator = FindObjectOfType<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMoving = true;
            FindPath();
        }

        Debug.Log(destinationInfo.point);
        IsPlayerMoving();
        //Rotate();
        //Move();

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

    private void IsPlayerMoving()
    {
        if (Vector3.Distance(transform.position, destinationInfo.point) <= minDistance)
        {
            Debug.Log(Vector3.Distance(transform.position, destinationInfo.point));
            isMoving = false;
        }

        if (isMoving)
        {
            animator.SetBool(isWalkingBoolName, true);
        }
        else
        {
            animator.SetBool(isWalkingBoolName, false);
        }
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        var moveDirection = transform.forward * moveVertical + transform.right * moveHorizontal;

        if (moveVertical > 0f)
        {
            animator.SetBool(isWalkingBoolName, true);
        }
        else if (moveVertical == 0f)
        {
            animator.SetBool(isWalkingBoolName, false);
        }

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        moveDirection.y = Physics.gravity.y;


        characterController.Move(moveDirection * (speed * Time.deltaTime));
    }

    private void Rotate()
    {
        var h = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up, rotationSpeed*h*Time.deltaTime);

    }

}
