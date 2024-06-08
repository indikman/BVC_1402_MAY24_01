using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
   
    [SerializeField]
    List<GameObject> TheFood = new List<GameObject>();

    public static GameManager Instance;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }
    
    private GameTimer _gameTimer;
    private int _score;

    [SerializeField] private TMP_Text scoreText;
    
    void Start()
    {

        TheFood.Add(new GameObject("Food1")); 
        

        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.StartTimer();
    }

    public void AddScore(int value,Vector3 SpawnFoodPoint)
    {
        _score += value;
        scoreText.text = _score.ToString();

        StartCoroutine(FoodSpawn(SpawnFoodPoint)); 
    }
    IEnumerator FoodSpawn(Vector3 SpawnFoodPoint)
    {

        yield return new WaitForSeconds(3f);
        Instantiate(TheFood.ElementAt(Random.RandomRange(0,4)), SpawnFoodPoint,Quaternion.identity);

    }
    
}
