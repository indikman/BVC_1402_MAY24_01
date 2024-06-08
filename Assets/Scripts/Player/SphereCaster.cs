using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCaster : MonoBehaviour
{
    public GameObject currentHitObject;
    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;


    CharacterController charCtrl;

    private Vector3 origin;
    private Vector3 direction;

    private float currentHitDistance;
    // Start is called before the first frame update
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Vector3 p1 = transform.position + charCtrl.center;
        float distanceToObstacle = 0;

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        if (Physics.SphereCast(p1, charCtrl.height / 2, transform.forward, out hit, 10))
        {
            distanceToObstacle = hit.distance;
        }
    }

    
}
