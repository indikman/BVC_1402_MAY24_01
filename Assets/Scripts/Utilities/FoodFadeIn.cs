using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class FoodFadeIn : MonoBehaviour
{
    [SerializeField]
    private float desiredAlpha;
    [SerializeField]
    private float currentAlpha;
    [SerializeField]
    private float fadeSpeed = 0.2f; //we want this to happen over 5 seconds 

    Collider collider;
    Renderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, fadeSpeed * Time.fixedDeltaTime);
        renderer.material.SetColor("_Color", new Color(1, 1, 1, currentAlpha));
        if (desiredAlpha - currentAlpha < 0.1)
        {
            collider.enabled = true;
            currentAlpha = desiredAlpha; 
            renderer.material.SetColor("_Color", new Color(1, 1, 1, desiredAlpha));
            renderer.material.ToOpaqueMode();
            this.enabled = false;
        }

    }
}
