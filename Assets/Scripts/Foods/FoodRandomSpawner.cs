using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodRandomSpawner : MonoBehaviour
{
    [Header("Food_List")]
    [SerializeField] List<GameObject> foodList;

    [Header("Spawn_Range")]
    [SerializeField] float _xSpawnRangeMin = -10f;
    [SerializeField] float _xSpawnRangeMax = 11f;
    [SerializeField] float _ySpawnRangeMin = -10f;
    [SerializeField] float _ySpawnRangeMax = 11f;

    private IEnumerator Spawn()
    {
        while (true)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(_xSpawnRangeMin, _xSpawnRangeMax), 1, Random.Range(_ySpawnRangeMin, _ySpawnRangeMax));
            Instantiate(foodList[Random.Range(0, foodList.Count)], randomSpawnPosition, Quaternion.identity);
            Debug.Log("Got something for you!");
            yield return new WaitForSeconds(5);
        }
    }
    void Start()
    {
        StartCoroutine(Spawn());
    }
}
