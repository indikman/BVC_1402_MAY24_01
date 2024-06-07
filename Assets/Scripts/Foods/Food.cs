using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    bool _hit = false;
    public int Value { get; protected set; }

    [SerializeField] float respawnTime = 5f;
    Vector3 originalPosition;

    BoxCollider itemCollider;

    private void Awake()
    {
        originalPosition = transform.position;
        itemCollider = GetComponentInChildren<BoxCollider>();
    }
    void Start()
    {
        originalPosition = transform.position;
        
        StartCoroutine(RespawnItemCoroutine());
    }

    IEnumerator RespawnItemCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            RespawnItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {;
            GameManager.Instance.AddScore(Value);
            CollectItem();
    }

    void CollectItem()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RespawnItem), 5f);
    }

    void RespawnItem()
    {
        gameObject.SetActive(true);
        itemCollider.enabled = true;
        transform.position = originalPosition;
    }
}
