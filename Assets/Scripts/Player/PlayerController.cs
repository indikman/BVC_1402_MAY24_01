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
    private float _movementAmount;
    private bool _isGround;
    private float _strafeMovement;

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

        if (velocity.magnitude > 0.1f && _strafeMovement < -0.1f)
        {
            animator.SetBool("isStrafingLeft", true);
            animator.SetBool("isStrafingRight", false);
            animator.SetBool("isRunning", false);
        }
        else if (velocity.magnitude > 0.1f && _strafeMovement > 0.1f)
        {
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", true);
            animator.SetBool("isRunning", false);
        }
        else if (velocity.magnitude > 0.1f)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", false);
        }
           
        
        animator.SetBool("isGround", _isGround);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement + _cameraObject.right * _strafeMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection *= walkSpeed;

        moveDirection.y = _rb.velocity.y;

        _rb.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement;
        if (_strafeMovement != 0)
        {
            targetDirection = _cameraObject.forward;
        }
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

    public void Jump()
    {
        if(!_isGround) return;
        
        animator.SetTrigger("jump");
        
        Vector3 currentVelocity = _rb.velocity;
        currentVelocity.y = jumpVelocity;

        _rb.velocity = currentVelocity;
    }

    private void GroundCheck()
    {
        _isGround = Physics.CheckSphere(transform.position + groundCheckPoint, radius, groundLayer);
    }

    public void SetStrafeInput(float input)
    {
        _strafeMovement = input;
    }

}
