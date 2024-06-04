using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCollider : MonoBehaviour
{
    [SerializeField]
    private float minDistance = 1.0f;
    [SerializeField]
    private float maxDistance = 4.0f;
    [SerializeField]
    private float smooth = 10f;
    Vector3 dollyDir;
    [SerializeField]
    private Vector3 dollyDirAdjustment;
    [SerializeField]
    private float distance;


    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }


    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if(Physics.Raycast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
