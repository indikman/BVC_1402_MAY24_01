using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCollider : MonoBehaviour
{
    [SerializeField]
    private float _minDistance = 1.0f;
    [SerializeField]
    private float _maxDistance = 4.0f;
    [SerializeField]
    private float _zoomCamera = 10f;
    Vector3 _dollyDir;
    [SerializeField]
    private float _distance;


    private void Awake()
    {
        _dollyDir = transform.localPosition.normalized;
        _distance = transform.localPosition.magnitude;
    }


    void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(_dollyDir * _maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            _distance = Mathf.Clamp((hit.distance * 0.9f), _minDistance, _maxDistance);
        }
        else
        {
            _distance = _maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, _dollyDir * _distance, Time.deltaTime * _zoomCamera);
    }
}
