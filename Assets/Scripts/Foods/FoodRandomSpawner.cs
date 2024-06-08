using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodRandomSpawner : MonoBehaviour
{
    [Header("Food_List")] //list of the food items
    [SerializeField] List<GameObject> foodList;

    [Header("Spawn_Range")] //set up the size of the spawning area
    [SerializeField] float _xSpawnRangeMin = -10f;
    [SerializeField] float _xSpawnRangeMax = 11f;
    [SerializeField] float _ySpawnRangeMin = -10f;
    [SerializeField] float _ySpawnRangeMax = 11f;

    private IEnumerator Spawn()
    {
        while (true)//start repeating
        {
            //set up spawning position randomly 
            Vector3 randomSpawnPosition = 
                new Vector3(Random.Range(_xSpawnRangeMin, _xSpawnRangeMax), 1, Random.Range(_ySpawnRangeMin, _ySpawnRangeMax));

            //instantiate for spawning an item from the food list randomly
            Instantiate(foodList[Random.Range(0, foodList.Count)], randomSpawnPosition, Quaternion.identity);

            Debug.Log("Got something for you!");//debug to check if spawning is successful

            yield return new WaitForSeconds(5);//wait 5 seconds before spawning next random item at random position
        }
    }
    void Start()
    {
        StartCoroutine(Spawn());//call the method Spawn when game starts
    }
}
