using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    float respawnTime = 5f; 
    [SerializeField]  
    float respawnDuration = 5f; 
    private Vector3 originalScale; 

    bool _hit = false;
    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }
    void Awake()
    {
            Collider foodCollider = GetComponent<Collider>();
            foodCollider.isTrigger = true;
            Value = GameConstants.BaseFoodValue;
            originalScale = transform.localScale; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hit)
        {
            GameManager.Instance.AddScore(Value);
            
            _hit = true;
            Renderer renderer = GetComponent<Renderer>();
            Collider collider = GetComponent<Collider>();
            renderer.enabled = false;
            collider.enabled = false;
            StartCoroutine(RespawnCoroutine());
        }
    }
    
    private IEnumerator RespawnCoroutine()
    {
        Renderer renderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();
  

        yield return new WaitForSeconds(respawnTime);
        
        //Scaling Logic
        renderer.enabled = true;
        float elapsedTime = 0;
        while (elapsedTime < respawnDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, (elapsedTime / respawnDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //TODO: Set local scale = original scale
        transform.localScale = originalScale;

        //renderer.enabled = false;
        renderer.enabled = true;

        _hit = false;
        collider.enabled = true;
    }
}
