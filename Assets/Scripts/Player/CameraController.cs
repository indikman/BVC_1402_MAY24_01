using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camera Speeds")]
    [SerializeField] private float followSpeed = 0.2f;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float pivotSpeed = 2f;

    [Header("Camera Pivot")]
    [SerializeField] private float minPivot = -35f;
    [SerializeField] private float maxPivot = 35f;
    [SerializeField] private Transform pivot;

    [Header("Camera Collision")]
    [SerializeField] private LayerMask obstructions; // LayerMask for obstructions
    [SerializeField] private float buffer = 0.2f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float zoomRate = 0.5f;
    [SerializeField] private float minCollision = 0.5f; // Minimum distance to consider for collision

    private Vector3 followVelocity = Vector3.zero;
    private float lookAngle = 0f;
    private float pivotAngle = 0f;
    private float zoomDistance;
    private float initialZoom;
    private Camera mainCam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCam = GetComponentInChildren<Camera>();
        initialZoom = mainCam.transform.localPosition.z;
        zoomDistance = initialZoom;
    }

    void LateUpdate()
    {
        FollowTarget();
        HandleCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position, ref followVelocity, followSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera(Vector2 input)
    {
        lookAngle += input.x * lookSpeed;
        pivotAngle -= input.y * pivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Quaternion targetRotation = Quaternion.Euler(0, lookAngle, 0);
        transform.rotation = targetRotation;

        Quaternion pivotRotation = Quaternion.Euler(pivotAngle, 0, 0);
        pivot.localRotation = pivotRotation;
    }

    private void HandleCollision()
    {
        Vector3 Position = pivot.TransformPoint(new Vector3(0, 0, zoomDistance));
        RaycastHit hit;

        // Perform a Raycast to detect obstacles
        bool hitDetected = Physics.Raycast(pivot.position, Position - pivot.position, out hit, Mathf.Abs(initialZoom), obstructions);

        if (hitDetected && hit.distance > minCollision)
        {
            // Calculate the adjusted distance considering obstructions
            float ChangedDistance = hit.distance - buffer;
            ChangedDistance = Mathf.Clamp(ChangedDistance, minDistance, maxDistance);

            // Log the hit details and the adjusted distance
            Debug.Log($"Hit detected: {hit.collider.name}, Hit distance: {hit.distance}, Adjusted distance: {ChangedDistance}");

            // Smoothly interpolate the zoom distance
            zoomDistance = Mathf.Lerp(zoomDistance, ChangedDistance, zoomRate * Time.deltaTime);
        }
        else
        {
            
            Debug.Log("No hit detected or hit distance is too short, reverting to initial zoom");

            // Smoothly interpolate the zoom distance back to the initial zoom
            zoomDistance = Mathf.Lerp(zoomDistance, initialZoom, zoomRate * Time.deltaTime);
        }

        // Move the main camera to the new adjusted position
        mainCam.transform.localPosition = new Vector3(0, 0, zoomDistance);

        // Log the final camera position
        Debug.Log($"Camera position adjusted to: {mainCam.transform.localPosition}");
    }
}