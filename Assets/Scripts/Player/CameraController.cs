using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
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


    #region Camera Collider variable
    private Transform player;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] float sphereRadius;
    private LayerMask Ground;
    private Vector3 desiredPosition;
    private float currentDistance;

    #endregion
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance =maxDistance;
        player = GetComponent<Transform>();
        OnDrawGizmos();
    }
    
    void Update()
    {
        FollowTarget();
    }
    private void LateUpdate()
    {
        desiredPosition = player.position -player.forward * currentDistance;
        Vector3 directionToPlayer =(player.transform.position -transform.position).normalized;
        Debug.Log("PlayerPosition" + player.transform.position);
        Debug.Log("CameraPosition" + transform.position);
        Debug.Log("directionToPlayer" + player.transform.position);
        
        Debug.Log("directionToPlayer" + directionToPlayer);
        if (Physics.SphereCast(player.position, sphereRadius, directionToPlayer, out RaycastHit hit, maxDistance,Ground))
        {
            Debug.Log("get obstale"+ hit.distance);
            Debug.Log("get Mask" + Ground);
            currentDistance = Mathf.Clamp(hit.distance,minDistance,maxDistance);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, maxDistance, smoothSpeed * Time.deltaTime);
            Debug.Log("not get obstale return" + maxDistance);
        }
        desiredPosition =player.position - player.forward*currentDistance;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(transform.position);
    }
    private void OnDrawGizmos()
    {
        if(player!= null)
        {
            Gizmos.color = Color.yellow;
            Vector3 directionToPlayer = (player.position -transform.position).normalized;
            Gizmos.DrawWireSphere(player.position - directionToPlayer * currentDistance, sphereRadius);
        }
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
