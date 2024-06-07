using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    bool _hit = false;
    private float _coolDown = 3f;
    private float _time;
    private float _alphaSpeed = 3f;

    public int Value
    {
        get; protected set; //what does it mean for a value to be protected?
    }
    void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Grabbable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(Value);
            Destroy(gameObject);
        }
    }

    private void Grabbable()
    {
        float alpha = this.GetComponent<Renderer>().material.color.a; //Get float of alpha that "This object" used

        if (alpha < 1f) //Check alpha, Because Hard spawn food don't need to re-appear
        {
            Material _showingUp = this.GetComponent<Renderer>().material; //Get material of object that they used
            _time += Time.deltaTime; //Set cool down time of food
            float t = _time / _alphaSpeed; //get speed of time to re-appear food
            _showingUp.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, t)); //setup re-appear food by increase transparenc
            
            Debug.Log("Transparent");
        }
        else
        {
            Debug.Log("Showing Up");
        }

        if (alpha == 1) //if food re-appear keep cool down for 3s, player can't grab food at this time
        {
            if (gameObject.GetComponent<Collider>().enabled == false) //check collider of food
            {
                _coolDown -= Time.fixedDeltaTime; //set cool down time
                if (_coolDown < 0)
                {
                    gameObject.GetComponent<Collider>().enabled = true; //when cool down time = 0, player can take it!
                }
            }
        }
    }

}
