using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Transform _cameraObject;
    private Rigidbody _rb;

    [Header("Movement")]    //player movement variables
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private bool isStrafing = false;

    [Header("Jump")]    //player jumping variables
    [SerializeField] private float jumpVelocity = 20f;

    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animations")]  //animator
    [SerializeField] private Animator animator;

    private float _xMovement;
    private float _yMovement;
    private float _strafingMovement;
    private float _movementAmount;
    private bool _isGround;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (!isStrafing)
        {
            HandleMovement();
            HandleRotation();
        }
        else
        {
            HandleStrafing();
        }
    }

    private void Update()
    {
        GroundCheck();

        var velocity = _rb.velocity;
        velocity.y = 0;

        if (velocity.magnitude > 0.1f)//if the player is moving
        {
            if (!isStrafing)    //if the player is not strafing
            {
                animator.SetBool("isRunning", true);//then he is running
                animator.SetBool("isStrafingLeft", false);
                animator.SetBool("isStrafingRight", false);
            }
            else
            {
                if (velocity.x < 0.1f)//if the player is strafing left
                {
                    animator.SetBool("isStrafingLeft", true);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isStrafingRight", false);
                }

                else if (velocity.x > 0.1f) //if the player is strafing right
                {
                    animator.SetBool("isStrafingRight", true);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isStrafingLeft", false);
                }
            }
        }
        else
        {
            //since the player is not moving, then there is no animation for the player (except the idle animation)
            animator.SetBool("isRunning", false);
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", false);
        }
        
        animator.SetBool("isGround", _isGround);
        
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
    public void HandleStrafing()
    {
        Vector3 moveDirection = gameObject.transform.right * _strafingMovement;


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
        _yMovement = input.y;
        isStrafing = false;
    }
    public void SetStrafingInput(Vector2 input)
    {
        _strafingMovement = input.x;
        isStrafing = true;
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
