using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

public class Food : MonoBehaviour
{
    #region for respawnFood

    [SerializeField] private GameObject[] foodPrefabs;
    [SerializeField] private Vector3 spawnAreaMin;
    [SerializeField] private Vector3 spawnAreaMax;
    [SerializeField] private float respawnDuration = 5f;
    [SerializeField] private float grabbableDelay = 2.0f; 
    #endregion
    
    void Start()
    {
        StartCoroutine(RespawnSpawnItemsWithDelay());
    }

    IEnumerator RespawnSpawnItemsWithDelay()
    {
        Debug.Log("respawn with delay" );
        while (true)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject foodPrefab = GetRandomFoodPrefab();
            GameObject foodInstance = Instantiate(foodPrefab, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(respawnDuration);

            Collider foodCollider = foodInstance.GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.enabled = false;
                StartCoroutine(EnableGrabbableAfterDelay(foodCollider, grabbableDelay));
            }

           
        }
    }

    IEnumerator EnableGrabbableAfterDelay(Collider foodCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
        foodCollider.enabled = true;
    }
    Vector3 GetRandomPosition()
    {
        float x = UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, y, z);
    }

    GameObject GetRandomFoodPrefab()
    {

        int randomIndex = UnityEngine.Random.Range(0, foodPrefabs.Length);
        Debug.Log("Length" + foodPrefabs.Length);
        return foodPrefabs[randomIndex];
    }

    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }
    void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(Value);
            Destroy(gameObject);
          
           
        }
    }
}
