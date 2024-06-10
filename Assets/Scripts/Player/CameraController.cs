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

    [Header("Camera Collision")]
    [SerializeField] private LayerMask obstructionLayers; // LayerMask for obstructions
    [SerializeField] private float collisionBuffer = 0.2f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 5f;
    [SerializeField] private float zoomLerpRate = 0.5f;
    [SerializeField] private float sphereCastRadius = 0.2f;
    [SerializeField] private float minCollisionDistance = 0.5f; // Minimum distance to consider for collision

    private Vector3 _cameraFollowVelocity = Vector3.zero;
    private float _lookAngle = 0f;
    private float _pivotAngle = 0f;
    private float _zoomDistance;
    private float _initialZoom;
    private Camera _mainCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mainCamera = GetComponentInChildren<Camera>();
        _initialZoom = _mainCamera.transform.localPosition.z;
        _zoomDistance = _initialZoom;
    }

    void LateUpdate()
    {
        FollowTarget();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera(Vector2 mouseInput)
    {
        _lookAngle += mouseInput.x * cameraLookSpeed;
        _pivotAngle -= mouseInput.y * cameraPivotSpeed;
        _pivotAngle = Mathf.Clamp(_pivotAngle, minPivotAngle, maxPivotAngle);

        Quaternion targetRotation = Quaternion.Euler(0, _lookAngle, 0);
        transform.rotation = targetRotation;

        Quaternion pivotRotation = Quaternion.Euler(_pivotAngle, 0, 0);
        cameraPivot.localRotation = pivotRotation;
    }

    private void HandleCameraCollision()
    {
        Vector3 intendedPosition = cameraPivot.TransformPoint(new Vector3(0, 0, _zoomDistance));
        RaycastHit hit;

        // Perform a SphereCast to detect obstacles
        bool hitDetected = Physics.SphereCast(cameraPivot.position, sphereCastRadius, intendedPosition - cameraPivot.position, out hit, Mathf.Abs(_initialZoom), obstructionLayers);

        if (hitDetected && hit.distance > minCollisionDistance)
        {
            // Calculate the adjusted distance considering obstructions
            float adjustedDistance = hit.distance - collisionBuffer;
            adjustedDistance = Mathf.Clamp(adjustedDistance, minZoom, maxZoom);

            // Smoothly interpolate the zoom distance
            _zoomDistance = Mathf.Lerp(_zoomDistance, adjustedDistance, zoomLerpRate * Time.deltaTime);
        }
        else
        {
            // Smoothly interpolate the zoom distance back to the initial zoom
            _zoomDistance = Mathf.Lerp(_zoomDistance, _initialZoom, zoomLerpRate * Time.deltaTime);
        }

        // Move the main camera to the new adjusted position
        _mainCamera.transform.localPosition = new Vector3(0, 0, _zoomDistance);
    }
}
