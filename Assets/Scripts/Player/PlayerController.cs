using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Transform _cameraObject;
    private Rigidbody _rb;

    [SerializeField]
    GameObject _cameraPivot;
    [SerializeField]
    GameObject _cameraDefault;
    

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


        RaycastHit hit;
      
        if (Physics.Raycast(_cameraPivot.transform.position, _cameraObject.transform.position - _cameraPivot.transform.position, out hit))
            if(hit.collider != null)
            {
                if (hit.collider.tag == "Ground")
                {
                    if (hit.distance <= (this.transform.position - _cameraDefault.transform.position).magnitude)
                    {
                        _cameraObject.transform.position = UnityEngine.Vector3.Slerp(_cameraObject.transform.position, hit.point, 0.1f);
                    }
                    else
                    {
                        _cameraObject.transform.position = UnityEngine.Vector3.Slerp(_cameraObject.transform.position, _cameraDefault.transform.position, 0.1f);

                    }
                }
            }
                
 
        if (hit.collider == null)
        {
            _cameraObject.transform.position = UnityEngine.Vector3.Slerp(_cameraObject.transform.position, _cameraDefault.transform.position, 0.1f);
            
        }
        
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
        float _strafeXMovement;
        if (Input.GetKey(KeyCode.Q))
        {
            _strafeXMovement = -1;
             moveDirection += _cameraObject.right * _strafeXMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            moveDirection *= walkSpeed;
            moveDirection.y = _rb.velocity.y;
            _rb.velocity = moveDirection;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _strafeXMovement = 1;
             moveDirection += _cameraObject.right * _strafeXMovement;
            moveDirection.Normalize();
            moveDirection.Normalize();
            moveDirection *= walkSpeed;
            moveDirection.y = _rb.velocity.y;
            _rb.velocity = moveDirection;
        }

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
