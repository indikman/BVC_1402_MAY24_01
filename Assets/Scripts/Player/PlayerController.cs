using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform _cameraObject;
    private Rigidbody _rb;

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
    private float _strafeMovement; // Added strafing variable
    private float _movementAmount;
    private bool _isGround;
    private bool _isStrafing; // Added strafing state
    private bool _isStrafingLeft; // Added strafing left state
    private bool _isStrafingRight; // Added strafing right state

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

        if (_isStrafingLeft)
        {
            animator.SetBool("isStrafingLeft", true);
            animator.SetBool("isStrafingRight", false);
        }
        else if (_isStrafingRight)
        {
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", true);
        }
        else
        {
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", false);
        }

        if (velocity.magnitude > 0.1f && !_isStrafing)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        animator.SetBool("isGround", _isGround);

        // Reset strafing if no input
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            SetStrafe(0);
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection += _strafeMovement * this.transform.right;
        moveDirection.Normalize();

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
        _yMovement = input.y;
    }

    public void SetStrafe(float value)
    {
        _strafeMovement = value;
        if (value == 0)
        {
            _isStrafing = false;
            _isStrafingLeft = false;
            _isStrafingRight = false;
        }
        else
        {
            _isStrafing = true;
            if (value < 0)
            {
                _isStrafingLeft = true;
                _isStrafingRight = false;
            }
            else if (value > 0)
            {
                _isStrafingLeft = false;
                _isStrafingRight = true;
            }
        }
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
