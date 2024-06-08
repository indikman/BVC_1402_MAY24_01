using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("----Spawner----")]
    [SerializeField]
    private GameObject[] foodPrefabs;

    [Header("----Raycast Check----")]
    [SerializeField]
    private float distance;
    [SerializeField]
    private LayerMask layerMaskCheck;

    private bool _canSpawn;
    private Vector3 pos;
    private GameObject foodSpawn;
    private Coroutine _spawnFoods;

    private float _coolDown = 3f;

    private Material _foodMat;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFoods());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnFoods() //Coroutine HERE!!
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 9)); //wait until 5 to 8. Then spawn for each spawn
            int foodLenght = foodPrefabs.Length - 1;
            int foodCount = Random.Range(0, foodLenght);
            RaycastHit hit;
            _canSpawn = false;
            while (!_canSpawn)
            {
                Vector3 origin = new Vector3(Random.Range(-35f, 35f), 0, Random.Range(-35f, 35f)); //random position of origin
                if (Physics.Raycast(origin + new Vector3(0f, 1f, 0f), Vector3.down, out hit, distance)) //detect ground by Raycast
                {
                    pos = hit.point; //set point of raycast that hit the ground to instantiate foods
                    _canSpawn = true;
                }
                else
                {
                    _canSpawn = false;
                    //Debug.Log("No ground here");
                }
            }
            foodSpawn = Instantiate(foodPrefabs[foodCount], pos + new Vector3(0, Random.Range(0.3f, 2.1f), 0), Quaternion.identity, transform);
            foodSpawn.GetComponent<Collider>().enabled = false; //turn off collider of respawn prefab
            _foodMat = foodSpawn.GetComponent<Renderer>().material;
            _foodMat.color = new Color(1, 1, 1, 0); //Set transparency of respawn prefab
            //Debug.Log("Spawn");
        }
    }
}
