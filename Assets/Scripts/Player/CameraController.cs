using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _cameraFollowVelocity = Vector3.zero;
    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    private float cameraFollowSpeed = .2f;
    [SerializeField]
    private float cameraLookSpeed = 2f;
    [SerializeField]
    private float cameraPivotSpeed = 2f;
    [SerializeField]
    float minPivotAngle = -35;
    [SerializeField]
    float maxPivotAngle = 35;
    [SerializeField]
    Transform cameraPivot;


    private float _lookAngle = 0, _pivotAngle = 0;
    void Awake()
    {
        targetTransform = FindObjectOfType<PlayerController>().transform;
    }
    private void HandleAllCameraMovement()
    {
        FollowTarget();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref _cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera(Vector2 movement)
    {

    }
    private void LateUpdate()
    {
        HandleAllCameraMovement();
    }
}
