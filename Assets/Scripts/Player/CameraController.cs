using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Camera collision variables
    private Camera camera;
    [SerializeField] private LayerMask collisionMask;
    private float initialDistance;
    [SerializeField] private float collisionOffset  = 0.2f;
    [SerializeField] private float minDistanceToTarget  = 1f;
    private float zoomDistance;
    [SerializeField] private float zoomSmoothing  = 0.5f; // Smoothness parameter for zooming in and out


    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerController>().transform;
        camera = GetComponentInChildren<Camera>();
        initialDistance = camera.transform.localPosition.z;
        zoomDistance = initialDistance;
    }
    
    private void HandleCameraCollision()
    {
        //Vector3 intendedCameraPosition = cameraPivot.TransformPoint(new Vector3(0, zoomDistance, 0));

        Vector3 intendedCameraPosition = cameraPivot.TransformPoint(new Vector3(0, 0, zoomDistance));
    
        RaycastHit collisionHit;
        bool hasCollision = Physics.Linecast(cameraPivot.position, intendedCameraPosition, out collisionHit, collisionMask);
        float adjustedDistance = hasCollision ? collisionHit.distance - collisionOffset : initialDistance;

        if (hasCollision && adjustedDistance < minDistanceToTarget)
        {
            // adjustedDistance = 0;
            
            
            adjustedDistance = minDistanceToTarget;
        }

        zoomDistance = Mathf.Lerp(zoomDistance, adjustedDistance, zoomSmoothing * Time.deltaTime);

        camera.transform.position = cameraPivot.TransformPoint(new Vector3(0, 0, zoomDistance));
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        FollowTarget();
        HandleCameraCollision();
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
