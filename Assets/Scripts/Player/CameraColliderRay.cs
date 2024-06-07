using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderRay : MonoBehaviour
{

    public float minDistance = 1f;
    public float maxDistance = 4f;
    [SerializeField]
    public float smooth = 10f;
    Vector3 cameraPos;
    public float distance;
    // Start is called before the first frame update
    void Awake()
    {
        cameraPos = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = transform.parent.TransformPoint (cameraPos * maxDistance);
        RaycastHit hit;
        if(Physics.Linecast (transform.parent.position, desiredPosition, out hit))
        {
            distance = Mathf.Clamp ((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, cameraPos * distance, Time.deltaTime * smooth);
    }
}
