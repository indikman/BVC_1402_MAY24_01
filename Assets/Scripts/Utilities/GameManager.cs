using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    void Update()
    {
        if (GetComponent<GameTimer>()._timerValue <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void AddScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }

    
}
