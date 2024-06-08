using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField]
    private float minDistance = 1.0f;
    [SerializeField]
    private float maxDistance = 4.0f;
    [SerializeField]
    private float smoothSpeed = 10f;

    private Vector3 dollyDirection;
    private float currentDistance;

    void Start()
    {
        dollyDirection = transform.localPosition.normalized;
        currentDistance = transform.localPosition.magnitude;
    }

    void LateUpdate()
    {
        AdjustCameraPosition();
    }

    private void AdjustCameraPosition()
    {
        Vector3 desiredPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredPosition, out hit))
        {
            currentDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * currentDistance, Time.deltaTime * smoothSpeed);
    }
}
