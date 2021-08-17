using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Gravity")]
    [SerializeField] private float gravityScale;

    private void Awake()
    {
        animator = FindObjectOfType<Animator>();
    }


    private void Update()
    {
        Rotate();
        Move();

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
