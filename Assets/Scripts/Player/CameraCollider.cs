using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [Header("Camera Collider")]
    [SerializeField]
    private float minDistance = 1.0f; //Minimum of Camera distance
    [SerializeField]
    private float maxDistance = 3.0f; //Maximum of Camera distance
    [SerializeField]
    private float smooth = 10f; //Zooming smooth parameter
    [SerializeField]
    private float sphereRadius = 1.0f; //Radius of spherecast
    [SerializeField]
    private float cameraCastDistance; //Distance of spherecast
    [SerializeField]
    LayerMask layerHit; //Layer that spherecast hit
    [SerializeField]
    private GameObject currentHitObject; //To check what object that spherecast hit

    private float distance; //set distance for camera to zoom in and out
    
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = transform.parent.position; //spherecast position
        Vector3 direction = transform.forward; //Direction of spherecast
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, -direction, out hit, cameraCastDistance, layerHit))
        {
            currentHitObject = hit.transform.gameObject; //show object that spherecast hit
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance); //set value of distance between Mindistance and Maxdistance to hit.distance
        }
        else
        {
            currentHitObject = null;
            distance = maxDistance; //If spherecast hit nothing set distance to Maxdistance
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.back * distance, Time.deltaTime * smooth); //Use Lerp to set camera zoom in with clamp in distance when spherecast hit something
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward, sphereRadius); //draw gizmos to check spherecast

    }
}
