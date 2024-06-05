using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    
    [Header("Camera Speeds")]
    [SerializeField]
    private float cameraFollowSpeed = 0.2f;
    [SerializeField]
    private float cameraLookSpeed = 2f; //Camera rotate speed
    [SerializeField]
    private float cameraPivotSpeed = 2f; //Camera up down speed

    [Header("Camera Pivot")]
    [SerializeField]
    private float minPivotAngle = -35f; //Minimum angle for up down camera
    [SerializeField]
    private float maxPivotAngle = 35f;//Maximum angle for up down camera
    [SerializeField]
    private Transform cameraPivot;    
    
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    private float _lookAngle = 0f; //Angle of rotate y camera
    private float _pivotAngle = 0f; //angle of rotate x camera
    
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
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed); //camera follow player with SmoothDamp
        transform.position = targetPosition;
    }

    public void RotateCamera(Vector2 mouseInput)
    {
        Vector3 lookRotation = Vector3.zero;
        Vector3 pivotRotation = Vector3.zero;
        
        //LOOKING
        //camera turn around player in Horizontal
        _lookAngle = _lookAngle + mouseInput.x * cameraLookSpeed;
        lookRotation.y = _lookAngle;

        Quaternion targetLookRotation = Quaternion.Euler(lookRotation);
        transform.rotation = targetLookRotation;
        
        //PIVOT
        //camera turn around player in Vertical
        _pivotAngle = _pivotAngle - mouseInput.y * cameraPivotSpeed;
        _pivotAngle = Mathf.Clamp(_pivotAngle, minPivotAngle, maxPivotAngle);
        pivotRotation.x = _pivotAngle;

        Quaternion targetPivotRotation = Quaternion.Euler(pivotRotation);
        cameraPivot.localRotation = targetPivotRotation;
    }
}
