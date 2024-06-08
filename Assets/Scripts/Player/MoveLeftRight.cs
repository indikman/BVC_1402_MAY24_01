using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float speed = 5f; // Speed at which the GameObject moves

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the horizontal input
        float move = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            move = -1f; // Move left
        }
        else if (Input.GetKey(KeyCode.E))
        {
            move = 1f; // Move right
        }

        // Apply the movement to the GameObject
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);
    }
}
