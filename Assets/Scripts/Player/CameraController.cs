using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
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
    
    [SerializeField] private GameObject ray;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float cameraHeight=0.8f;
    private float obstacleBuffer = 0.5f;
    [SerializeField] private float maxDistance;
    [SerializeField] private float defaultDistance;
    #endregion
    
    
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FollowTarget();
    }

    void LateUpdate()
    {
        //use raycast to check obstacle
        Vector3 targetPosition = playerTransform.position + new Vector3(0,cameraHeight,-defaultDistance);
        Ray ray = new Ray(playerTransform.position,targetPosition-playerTransform.position);
        LayerMask obstacleMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, obstacleMask))
        {
           // Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
            targetPosition = hit.point + hit.normal * obstacleBuffer;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition,
             ref _cameraFollowVelocity, smoothSpeed);
            transform.LookAt(playerTransform);
        }
        else
        {
            Debug.Log("not get obstacle with ray");
        }
        FollowTarget();
    }
    private void FollowTarget()
    {
        Debug.Log("keep the default camera set up");
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position,
            ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
       // transform.LookAt(playerTransform);
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
