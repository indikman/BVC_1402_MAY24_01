using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private bool _hit = false;
    private bool _isReappearing = false;
    private Renderer _renderer;
    private Collider _collider;
    

    [SerializeField] private int _value = 10;
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] private float respawnDuration = 5f; 
    [SerializeField] private float rotationSpeed = 50f; 

    public int Value
    {
        get { return _value; }
        protected set { _value = value; }
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        // Gira o item lentamente no eixo Y, mas só se não estiver reaparecendo
        if (!_isReappearing)
        {
            Debug.Log("Rotating...");
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        else
        {
            Debug.Log("Not rotating because _isReappearing is true.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hit)
        {
            GameManager.Instance.AddScore(Value);
            _hit = true;
            _renderer.enabled = false;
            _collider.enabled = false;
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        // Aguarda o tempo de reaparecimento
        yield return new WaitForSeconds(respawnTime);

        _isReappearing = true;
        float elapsedTime = 0f;
        float startAlpha = 0f;
        float endAlpha = 1f;

        _renderer.enabled = true;

        // Obter o material original para que possamos modificar a cor sem criar novos materiais
        Material material = _renderer.material;

        while (elapsedTime < respawnDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / respawnDuration);

            // Ajusta a transparência do material
            Color color = material.color;
            color.a = alpha;
            material.color = color;

            yield return null;
        }

        // Garanta que o item esteja totalmente visível após a animação de reaparecimento
        Color finalColor = material.color;
        finalColor.a = 1f;
        material.color = finalColor;

        _collider.enabled = true;
        _hit = false;
        _isReappearing = false;
    }
}