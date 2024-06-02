using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    
    bool _hit = false;
    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }
    void Awake()
    {
        
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
