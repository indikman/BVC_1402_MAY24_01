using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    
    [Header("Camera Speeds")]
    [SerializeField] private float cameraFollowSpeed = 0.2f;
    [SerializeField] private float cameraLookSpeed = 2f;
    [SerializeField] private float cameraPivotSpeed = 2f;

    [Header("Camera Pivot")]
    [SerializeField] private float minPivotAngle = -35f;
    [SerializeField] private float maxPivotAngle = 35f;
    [SerializeField] private Transform cameraPivot;
    
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    private float _lookAngle = 0f;
    private float _pivotAngle = 0f;


    #region Camera Collider variable
   // private Transform target;
    [SerializeField] private GameObject ray;
    [SerializeField] private Transform closePosition;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float defaultDistance;
    private Vector3 offset;
    
    
    private Vector3 desiredPosition;
    private float currentDistance;

    #endregion
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance =maxDistance;
      
       
        offset = transform.position- targetTransform.position;
        currentDistance = defaultDistance;
    }

    void Update()
    {
        FollowTarget();
       
        LayerMask obstacleMask = LayerMask.GetMask("Ground");
        offset = transform.position - targetTransform.position;
        Vector3 disiredPosition = targetTransform.position + offset.normalized*currentDistance;
        Vector3 direction = (targetTransform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, maxDistance, obstacleMask))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
            CloseFollowTarget();
        }
        else
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
            Debug.Log("not get obstacle with ray");
            currentDistance = maxDistance;
            FollowTarget();
        }
        desiredPosition = targetTransform.position;
        
        transform.position = desiredPosition;
        transform.LookAt(targetTransform.position);

    }
    private void FollowTarget()
    {
        Debug.Log("keep the default camera set up");
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position,
            ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void CloseFollowTarget()
    {
        Vector3 targetPosition =targetTransform.position + offset.normalized*currentDistance;
        Debug.Log("push forward camera now");
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition,
             ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.LookAt(targetTransform);
       
    }
    public void RotateCamera(Vector2 mouseInput)
    {
        Vector3 lookRotation = Vector3.zero;
        Vector3 pivotRotation = Vector3.zero;
        
        //LOOKING
        _lookAngle = _lookAngle + mouseInput.x * cameraLookSpeed;
        lookRotation.y = _lookAngle;

        Quaternion targetLookRotation = Quaternion.Euler(lookRotation);
        transform.rotation = targetLookRotation;
        
        //PIVOT
        _pivotAngle = _pivotAngle - mouseInput.y * cameraPivotSpeed;
        _pivotAngle = Mathf.Clamp(_pivotAngle, minPivotAngle, maxPivotAngle);
        pivotRotation.x = _pivotAngle;

        Quaternion targetPivotRotation = Quaternion.Euler(pivotRotation);
        cameraPivot.localRotation = targetPivotRotation;

    }
    
}
