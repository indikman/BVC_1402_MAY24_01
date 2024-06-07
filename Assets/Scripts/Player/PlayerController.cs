
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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

    float _strafeMovement;

    private float _movementAmount;
    private bool _isGround;
    public bool _isStrafing = false;
    [SerializeField]
    float cameraAdjustSpeed;

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
                        _cameraObject.transform.position = UnityEngine.Vector3.Slerp(_cameraObject.transform.position, hit.point, cameraAdjustSpeed);
                    }
                    else
                    {
                        _cameraObject.transform.position = UnityEngine.Vector3.Slerp(_cameraObject.transform.position, _cameraDefault.transform.position, cameraAdjustSpeed);

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
        

        if(_strafeMovement == 0)
        {
            _isStrafing = false;
          
        }
        else
        {
            _isStrafing = true;
           
        }

        if(velocity.magnitude > 0.1f && _isStrafing == false) 
            animator.SetBool("isRunning", true);
        else 
            animator.SetBool("isRunning", false);
        animator.SetBool("isStrafing", _isStrafing);
        animator.SetBool("isGround", _isGround);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraObject.forward * _yMovement + _cameraObject.right * _xMovement;
        moveDirection.Normalize();        
            moveDirection += this.transform.right * _strafeMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            moveDirection *= walkSpeed;
            moveDirection.y = _rb.velocity.y;
            _rb.velocity = moveDirection;
    }

    public void SetStrafe(float input)
    {
        _strafeMovement = input;
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
