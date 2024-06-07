using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] private float _minDistance = 1.0f;
    [SerializeField] private float _maxDistance = 4.0f;
    [SerializeField] private float _cameraSmooth = 7.5f;

    Vector3 cameraDirection;
    [SerializeField] private float _cameraDistance;

    private void Awake()
    {
        cameraDirection = transform.localPosition.normalized;
        _cameraDistance = transform.localPosition.magnitude;
    }


    void Update()
    {
        Vector3 cameraPosition = transform.parent.TransformPoint(cameraDirection * _maxDistance);
        RaycastHit scan;

        if (Physics.Linecast(transform.parent.position, cameraPosition, out scan))
        {
            _cameraDistance = Mathf.Clamp((scan.distance * 0.9f), _minDistance, _maxDistance);
        }
        else
        {
            _cameraDistance = _maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, cameraDirection * _cameraDistance, Time.deltaTime * _cameraSmooth);
    }
}
