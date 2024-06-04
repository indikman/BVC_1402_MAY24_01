using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Coroutine _respawnCollectable;
    [SerializeField]
    private float _respawnTime;
    [SerializeField]
    GameObject _burgerPrefab;
    [SerializeField]
    private float _burgerSpawnRadius;

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
    
    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(_respawnTime);
        Instantiate(_burgerPrefab, this.transform.position + new Vector3(Random.Range(-_burgerSpawnRadius, _burgerSpawnRadius), 0, Random.Range(-_burgerSpawnRadius, _burgerSpawnRadius)),Quaternion.identity);
        _respawnCollectable = StartCoroutine(RespawnTimer());
    }
    
    void Start()
    {
        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.StartTimer();
        _respawnCollectable = StartCoroutine(RespawnTimer());
    }

    public void AddScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
    }

    
}
