using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable : MonoBehaviour
{


    public GameObject collectablePrefab;
    public float respawnTime = 10f;
    public int numberOfCollectables = 10; // Number of collectables to manage

    private GameObject[] collectables;

    // Start is called before the first frame update
    void Start()
    {
        collectables = new GameObject[numberOfCollectables];
        SpawnCollectables();
    }


    void SpawnCollectables()
    {
        for (int i = 0; i < numberOfCollectables; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f)); // Random spawn position
            collectables[i] = Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(RespawnCollectable(collectables[i]));
        }
    }

    IEnumerator RespawnCollectable(GameObject collectable)
    {
        while (true)
        {   
            yield return new WaitForSeconds(respawnTime);
            collectable.SetActive(true); // Or you can instantiate a new collectable object if you prefer
        }
    }

    
}
