using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    public bool canIGo;
    private Coroutine _testCoroutine;
    [SerializeField]
    List<GameObject> foodSpawnList = new List<GameObject>();
    [SerializeField]
    List<GameObject> foodsToSpawn = new List<GameObject>();
    [SerializeField]
    GameObject spawnParent;
    private int numberOfPoints;
    private float spawnRadius = 10;
    [SerializeField]
    float spawnFrequency = 1;
    private void Start()
    {
        foreach (Transform child in spawnParent.transform)
        {
            numberOfPoints++;
            string nameOf = string.Format("ItemSpawnPoint ({0})", numberOfPoints);
            foodSpawnList.Add(GameObject.Find(nameOf));
        }
        _testCoroutine = StartCoroutine(TestIEnumerator());
    }
    public IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(spawnFrequency);
        Vector3 spawnPos = (foodSpawnList[Random.Range(0, foodSpawnList.Count)].transform.position);
        spawnPos += new Vector3(Random.Range(-spawnRadius, spawnRadius),0, Random.Range(-spawnRadius, spawnRadius));
        Instantiate(foodsToSpawn.ElementAt(Random.Range(0,foodsToSpawn.Count)),spawnPos, Quaternion.identity);
        StartCoroutine(SpawnItem());
    }
    private IEnumerator TestIEnumerator()
    {
        Debug.Log("Waiting until I am allowed!");
        yield return new WaitUntil(()=> canIGo);
        Debug.Log("Here I go!");
    }
  
}
