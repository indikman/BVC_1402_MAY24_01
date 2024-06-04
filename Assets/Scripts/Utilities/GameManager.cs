using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    List<GameObject> foodList = new List<GameObject>();
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
        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.StartTimer();
    }

    public void AddScore(int value, Vector3 respawnLocation)
    {
        _score += value;
        scoreText.text = _score.ToString();
        StartCoroutine(SpawnRoutine(respawnLocation));
    }
    IEnumerator SpawnRoutine(Vector3 respawnLocation)
    {
        yield return new WaitForSeconds(5);
        Instantiate(foodList.ElementAt(Random.Range(0,foodList.Count)),respawnLocation,Quaternion.identity);
    }
}