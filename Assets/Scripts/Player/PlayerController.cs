using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform _cameraObject;
    private Rigidbody _rb;

    bool shouldStrafe = false;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 20f;

    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    private float _xMovement;
    private float _yMovement;
    private float _movementAmount;
    private bool _isGround;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void Update()
    {
        GroundCheck();

        var velocity = _rb.velocity;
        velocity.y = 0;



        if (_rb.velocity.magnitude < 0.1f)
        {

            animator.SetBool("isRunning", false);

            animator.SetBool("isGround", _isGround);
        }


        if (shouldStrafe)
        {
            if (_xMovement > 0)
            {

                animator.SetBool("isStrafingRight", true);
            }
            else
            {
                animator.SetBool("isStrafingRight", false);

                animator.SetBool("isGround", _isGround);
            }

            if (_xMovement < 0)
            {

                animator.SetBool("isStrafingLeft", true);
            }
            else
            {
                animator.SetBool("isStrafingLeft", false);

                animator.SetBool("isGround", _isGround);
            }

            return;
        }

        if (_rb.velocity.magnitude > 0.1f)
        {

            animator.SetBool("isRunning", true);
        }


    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection *= walkSpeed;

        moveDirection.y = _rb.velocity.y;

        _rb.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void SetMovementInput(Vector2 input)
    {
        _xMovement = input.x;
        if (input.x != 0)
        {
            shouldStrafe = false;
        }
        if (input.y != 0)
        {
            shouldStrafe = false;
        }
        _yMovement = input.y;
    }

    public void SetStrafeInput(float input)
    {
        shouldStrafe = true;
        _xMovement = input;
    }

    public void Jump()
    {
        if (!_isGround) return;

        animator.SetTrigger("jump");

        Vector3 currentVelocity = _rb.velocity;
        currentVelocity.y = jumpVelocity;

        _rb.velocity = currentVelocity;
    }

    private void GroundCheck()
    {
        _isGround = Physics.CheckSphere(transform.position + groundCheckPoint, radius, groundLayer);
    }

}
