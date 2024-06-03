using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    //using  [Unity 5] Tutorial: How to spawn objects at random position in a given area https://www.youtube.com/watch?v=kTvBRkPTvRY
    [SerializeField]
    private Vector3 boundCenter;
    [SerializeField]
    private Vector3 boundSize;
    [SerializeField]
    private GameObject[] foodPrefabList;

    private GameObject spawnedPrefab;
    [SerializeField]
    private LayerMask layermask;
    [SerializeField]
    private float sphereRadius = 1;
    [SerializeField]
    private GameObject parentObject; //Where the parent collectable object is

    private bool goodPosition = false;
    [SerializeField]
    private int retryNum;

    void Start()
    {
        if (foodPrefabList.Length == 0)
        {
            Debug.Log("empty list of prefabs");
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    public GameObject SpawningFood()
    {
        Vector3 pos = boundCenter;
        int retrys = retryNum;
        goodPosition = false;
        while (goodPosition = false || retrys > 0)
        { 
            pos = boundCenter + new Vector3(Random.Range(-boundSize.x/2, boundSize.x/2),
            Random.Range(-boundSize.y / 2, boundSize.y / 2), 
            Random.Range(-boundSize.z / 2, boundSize.z / 2));
            //now to check if it collides with something.
            if (Physics.CheckSphere(pos, sphereRadius, layermask))
            {
                Debug.Log("Spawned position is colliding with something");
                goodPosition = false;
            }
            else
            {
                goodPosition = true;
            }
            retrys--;
        }
        spawnedPrefab = Instantiate(foodPrefabList[Random.Range(0, foodPrefabList.Length)], pos, Quaternion.identity, parentObject.transform);
        return spawnedPrefab;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(boundCenter, boundSize);
    }

}
