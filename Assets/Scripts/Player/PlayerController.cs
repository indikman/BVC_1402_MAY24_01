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
    [SerializeField] private float strafeSpeed = 1f;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 20f;

    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    private float _xMovement;
    private float _yMovement;
    private float _strafeInput;
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

        if (velocity.magnitude > 0.1f && _strafeInput == 0f) 
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        animator.SetBool("isGround", _isGround);

        // Captura os inputs de movimento padrão
        float horizontalInput = Input.GetAxis("Horizontal"); // Para A e D
        float verticalInput = Input.GetAxis("Vertical");

        // Atualiza o movimento no PlayerController
        SetMovementInput(new Vector2(horizontalInput, verticalInput));

        // Captura os inputs de strafe
        _strafeInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            _strafeInput = -1f;
            animator.SetBool("isStrafingLeft", true);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _strafeInput = 1f;
            animator.SetBool("isStrafingRight", true);
            Debug.Log("Right Strafe works");
        }
        else
        {
            _strafeInput = 0f;
            animator.SetBool("isStrafingLeft", false);
            animator.SetBool("isStrafingRight", false);
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement; // Inclui movimento para frente, trás, esquerda e direita
        moveDirection.Normalize();
        moveDirection *= walkSpeed;

        // Adiciona movimento lateral com base no input de strafe
        moveDirection += transform.right * _strafeInput * strafeSpeed;

        moveDirection.y = _rb.velocity.y;

        _rb.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement; // Inclui a direção para frente e para os lados
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