using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Transform _cameraObject;
    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] 
    private float walkSpeed = 1.5f;
    [SerializeField] 
    private float rotationSpeed = 15f;

    [Header("Jump")] 
    [SerializeField] 
    private float jumpVelocity = 20f;

    [SerializeField] 
    private Vector3 groundCheckPoint;
    [SerializeField] 
    private float radius; //radius of ground check
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Animations")]
    [SerializeField] 
    private Animator animator;

    private float _xMovement;
    private float _yMovement;
    private float _movementAmount;
    private float _xStrafing = 0f;
    private bool _isGround;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform; //get component from main camera
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
        
        //set conditions for running animation
        if(velocity.magnitude > 0.1f && _xStrafing == 0f) 
            animator.SetBool("isRunning", true);
        else 
            animator.SetBool("isRunning", false);
        
        //set animation for jump animation
        animator.SetBool("isGround", _isGround);

        //set animation for strafing animation
        if(velocity.magnitude > 0.1f && _xStrafing < 0)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isLeftStrafe", true);
            animator.SetBool("isRightStrafe", false);
        }
            
        else if (velocity.magnitude > 0.1f && _xStrafing > 0)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isLeftStrafe", false);
            animator.SetBool("isRightStrafe", true);
        }
        else
        {
            animator.SetBool("isLeftStrafe", false);
            animator.SetBool("isRightStrafe", false);
        }
            
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement + _cameraObject.right * _xStrafing; //main movement :_cameraObject.forward * _yMovement + _cameraObject.right * _xMovement + strafing movement : _cameraObject.right * _xStrafing;
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

        if(_xStrafing != 0 && _yMovement != -1)
        {
            Quaternion strafeRotation = Quaternion.Slerp(transform.rotation, _cameraObject.rotation, rotationSpeed * Time.deltaTime);
            strafeRotation = Quaternion.Euler(new Vector3(0f, strafeRotation.eulerAngles.y, 0f));

            transform.rotation = strafeRotation;
        }
    }

    public void SetMovementInput(Vector2 input)
    {
        _xMovement = input.x;
        _yMovement = input.y;
    }

    public void SetStrafingInput(float value)
    {
        _xStrafing = value;
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
   
}
