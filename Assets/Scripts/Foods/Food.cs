using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private bool isCollectable = false;
    private Vector3 orignalScale;

    [SerializeField] private float respawnTime = 10f; // Duration before respawn
    [SerializeField] private float reappear = 5f; // Duration for smooth reappear
    [SerializeField] private int value = 10; // Default value for the collectable item

    public int Value
    {
        get { return value; }
        protected set { value = value; }
    }

    void Awake()
    {
        orignalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollectable)
        {
            GameManager.Instance.AddScore(Value);
            StartCoroutine(Respawning());
        }
    }

    private IEnumerator Respawning()
    {
        isCollectable = true;
        transform.localScale = Vector3.zero;
        Debug.Log("Item is respawning. Hiding item...");

        // Wait for the specified respawn time
        yield return new WaitForSeconds(respawnTime);
        Debug.Log("Respawn time has elapsed. Starting reappear animation...");

        // Initialize variables for lerping
        Vector3 targetScale = orignalScale;
        float elapsedTime = 0f;

        // Gradually increase scale over reappearDuration
        for(elapsedTime =0; elapsedTime < reappear; elapsedTime+= 0.02f) 
        {
            // Interpolate between zero scale and target scale
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / reappear);
            Debug.Log($"Item scale: {transform.localScale}");

            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        // Ensure the scale is set to the target scale and reset the respawning flag
        transform.localScale = targetScale;
        Debug.Log("Reappear animation completed. Item is fully visible.");
        isCollectable = false;
    }
}