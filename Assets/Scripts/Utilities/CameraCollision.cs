using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    /* This class is for the camera collider
     * With this class, the camera will follow the player correctly that the view won't be blocked 
     */

    [Header("Camera_Distance")]
    [SerializeField] float minDistance = 1.0f;
    [SerializeField] float maxDistance = 4.0f;

    [Header("Camera_OtherInput")]
    [SerializeField] float smooth = 10.0f;
    private Vector3 dollyDir;
    private Vector3 dollyDirAdjusted;
    private float distance;

    // Start is called before the first frame update
    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast (transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
