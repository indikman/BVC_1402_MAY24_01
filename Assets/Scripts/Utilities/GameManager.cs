using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CoroutineTest test;
   
    void Awake()
    {
        test = this.GetComponent<CoroutineTest>();
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
        StartCoroutine(test.SpawnItem());
        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.StartTimer();
    }
    public void Update()
    {
       
    }
    public void AddScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }

    
}
