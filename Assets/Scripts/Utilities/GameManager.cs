using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Coroutine _respawnCollectable;
    [SerializeField]
    private float _respawnTime;
    [SerializeField]
    public List<GameObject> _foodItems = new List<GameObject>();
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
        Instantiate(_foodItems.ElementAt(Random.Range(0,_foodItems.Count)), this.transform.position + new Vector3(Random.Range(-_burgerSpawnRadius, _burgerSpawnRadius), -0.5f, Random.Range(-_burgerSpawnRadius, _burgerSpawnRadius)),Quaternion.identity); 
    }
    
    void Start()
    {
        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.StartTimer();
    }

    public void AddScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();
        _respawnCollectable = StartCoroutine(RespawnTimer());
    }

    
}
