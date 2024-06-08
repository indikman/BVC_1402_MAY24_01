using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] public float minDistance = 1f;
    [SerializeField] public float maxDistance = 4f;
    [SerializeField] public float smooth = 10f;
    [SerializeField] public float distance;

    public Vector3 dollyDirAjusted;
    Vector3 dollyDir;


    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint (dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast (transform.parent.position, desiredCameraPos, out hit)) 
        {
            distance = Mathf.Clamp ((hit.distance * 0.87f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}

