using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.StartTimer();
    }

    public void AddScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }

    
}
