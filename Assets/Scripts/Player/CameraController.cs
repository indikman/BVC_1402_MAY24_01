using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform cameraPivot;

    [Header("Camera Speeds")]
    [SerializeField] private float cameraFollowSpeed = 0.2f;
    [SerializeField] private float cameraLookSpeed = 2f;
    [SerializeField] private float cameraPivotSpeed = 2f;

    [Header("Camera Pivot")]
    [SerializeField] private float minPivotAngle = -35f;
    [SerializeField] private float maxPivotAngle = 35f;
   

  


    private Vector3 _cameraFollowVelocity = Vector3.zero;

    private float _lookAngle = 0f;
    private float _pivotAngle = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        FollowTarget();
        
    }
   
 
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position,
            ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
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
