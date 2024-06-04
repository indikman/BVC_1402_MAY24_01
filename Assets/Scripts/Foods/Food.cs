using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Material foodMat;
    public Color customColor = new Color(0, 0, 0, 0);
    bool _hit = false;
    public bool canCollect = false;
    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }
    void Awake()
    {
        foodMat = this.GetComponent<MeshRenderer>().material;

        StartCoroutine(changeColor());

    }
    private IEnumerator changeColor()
    {
        yield return new WaitForSeconds(0.1f);
        if (customColor.a <= 1)
        {
            customColor.a += 0.02f;
            customColor.r += 0.02f;
            customColor.g += 0.02f;
            customColor.b += 0.02f;
            StartCoroutine(changeColor());
        }
        if (customColor.a >= 0.9f)
        {
            canCollect = true;
        }
    }
    void Update()
    {
        foodMat.SetColor("_Color", customColor);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.GetComponent<Cake>() != null)
            {
                if (this.gameObject.GetComponent<Cake>().canCollect)
                {
                    GameManager.Instance.AddScore(Value);
                    Destroy(gameObject);
                }
            }
            else
            {
                GameManager.Instance.AddScore(Value);
                Destroy(gameObject);
            }
            
        }
    }
}
