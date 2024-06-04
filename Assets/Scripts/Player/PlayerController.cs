using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

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

    [Header("Strafe")]
    private float _xstrafeInput;
    private float _ystrafeInput;

    [SerializeField] private float strafeSpeed = 1.5f;

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
        HandleStrafe();
    }

    private void Update()
    {
        GroundCheck();

        var velocity = _rb.velocity;
        velocity.y = 0;
        
        if(velocity.magnitude > 0.1f) 
            animator.SetBool("isRunning", true);
        else 
            animator.SetBool("isRunning", false);
        
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
        Debug.Log(input);
        _xMovement = input.x;
        _yMovement = input.y;
    }

    //this is for strafe movement
    public void SetStrafeInput(float input)// float not vector 2 just for simply
    {
     
        _xstrafeInput = input;
        _ystrafeInput = 0f;
        Debug.Log(input);//can get input already -1 and 1
    }
    private void HandleStrafe()
    {
        Vector2 move = new Vector2(_xstrafeInput, 0 ) * strafeSpeed * Time.deltaTime;
        if (_xstrafeInput == -1)
        {
            
            animator.SetBool("isStrafeL",true);
            animator.SetBool("isRunning", false);
            // animator.ResetTrigger("strafeR");
        }
 
        else if(_xstrafeInput == 1)
        {
            animator.SetBool("isStrafeR",true);
            //animator.ResetTrigger("strafeL");
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isStrafeR", false);
            animator.SetBool("isStrafeL", false);
        }
            

         transform.Translate(move);
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
