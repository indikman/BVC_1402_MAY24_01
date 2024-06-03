using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(Value);
            Debug.Log("food collected");
            GameManager.Instance.RespawnFood();
            Destroy(gameObject);
        }            
    }

}
