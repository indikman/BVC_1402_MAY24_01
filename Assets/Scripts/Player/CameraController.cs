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

    [Header("Camera Zoom")]
    [SerializeField] private Camera mainCameraObject;
    [SerializeField] private float zoomSpeed = .5f;
    [SerializeField] private float defaultZoom = -2.25f;
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private Vector3 playerCenter;
    [SerializeField] private float drawDis = 3f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultZoom = mainCameraObject.transform.position.z;
    }
    
    void Update()
    {
        FollowTarget();
        ZoomCameraWhenBlocked();
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

    private void ZoomCameraWhenBlocked()
    {
        RaycastHit hit;
        if (Physics.Raycast(targetTransform.position + playerCenter, (mainCameraObject.transform.TransformDirection(Vector3.back) + playerCenter), out hit, drawDis, raycastLayer))
        {
            //Debug.DrawRay(targetTransform.position + playerCenter, (mainCameraObject.transform.TransformDirection(Vector3.back) + playerCenter) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit: " + hit.collider.gameObject.name);
            mainCameraObject.transform.localPosition = Vector3.MoveTowards(mainCameraObject.transform.localPosition, Vector3.zero, zoomSpeed * Time.deltaTime);
            drawDis = Mathf.Abs(mainCameraObject.transform.localPosition.z);

        }
        else
        {
            //Debug.DrawRay(targetTransform.position + playerCenter, (mainCameraObject.transform.TransformDirection(Vector3.back) + playerCenter) * drawDis, Color.white);
            //Debug.Log("Did not Hit");
            mainCameraObject.transform.localPosition = Vector3.MoveTowards(mainCameraObject.transform.localPosition, new Vector3(0, 0, defaultZoom), zoomSpeed * Time.deltaTime);
            drawDis = Mathf.Abs(mainCameraObject.transform.localPosition.z);
        }
    }
    
}
