using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private bool _isRespawning = false;
    private Vector3 _originalScale;

    [SerializeField] private float respawnTime = 10f; // Duration before respawn
    [SerializeField] private float reappearDuration = 5f; // Duration for smooth reappear
    [SerializeField] private int value = 10; // Default value for the collectable item

    public int Value
    {
        get { return value; }
        protected set { value = value; }
    }

    void Awake()
    {
        _originalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isRespawning)
        {
            GameManager.Instance.AddScore(Value);
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        _isRespawning = true;
        transform.localScale = Vector3.zero; // Hide the item
        yield return new WaitForSeconds(respawnTime);

        Vector3 targetScale = _originalScale;
        float elapsedTime = 0f;

        // Gradually increase scale over reappearDuration
        while (elapsedTime < reappearDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / reappearDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        _isRespawning = false;
    }
}
