using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AnimatorController _animatorController;
    Vector3 _moveDirection;
    Transform _cameraObject;
    Rigidbody _rb;
    [Header("Movement")]
    [SerializeField]
    private float rotationSpeed = 15f;
    [SerializeField]
    float walkSpeed = 1.5f;
    [SerializeField]
    float runSpeed = 5f;
    [SerializeField]
    float sprintSpeed = 7f;
    [Header("Falling")]
    [SerializeField]
    float rayCastHeightOffset = 0.1f;
    [SerializeField]
    LayerMask groundLayer;

    [Header("Jump info")]
    [SerializeField]
    float jumpForce = 20f;
    [Header("Input")]
    private float _xMovement;
    private float _yMovement;
    private float _movementAmount;
    private float _cameraMovementX;
    private float _cameraMovementY;
    
    bool isJumping;
    bool isSprinting;

    


    // Start is called before the first frame update
    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();    
        _rb = GetComponent<Rigidbody>();
        _cameraObject = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        
        HandleMovement();
        HandleRotation();
    }
    private void HandleInput()
    {
        HandleMovementInput();
        HandleJumpInput();

    }
    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement;
        moveDirection += _cameraObject.right * _xMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (isSprinting)
        {
            moveDirection = moveDirection * sprintSpeed;
        }
        else
        {
            if (_movementAmount >= 0.5f)
            {
                moveDirection = moveDirection * runSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkSpeed;
            }
        }
        moveDirection.y = _rb.velocity.y;
        _rb.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = _cameraObject.forward * _yMovement;
        targetDirection = targetDirection + _cameraObject.right * _xMovement;
        targetDirection.Normalize();
        targetDirection.y = 0;
        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }
    
    private void HandleMovementInput()
    {
        _xMovement = Input.GetAxis("Horizontal");
        _yMovement = Input.GetAxis("Vertical");
        _movementAmount = Mathf.Abs(_xMovement) + Mathf.Abs(_yMovement);
        _animatorController.UpdateMovementValues(0, _movementAmount, isSprinting);
    }
    private void HandleSprintInput()
    {
        
    }
    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Vector3 velocity = _rb.velocity;
            velocity.y = jumpForce;
            _rb.velocity = velocity;
        }
    }
}
