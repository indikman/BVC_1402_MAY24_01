using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : Food
{
    private Coroutine _waitUntil;
    private bool _isMoving;
    private float _timeUntilPickup = 5f;

    void Start()
    {
        Value = GameConstants.BurgerValue;
        GetComponent<BoxCollider>().enabled = false;
        _waitUntil = StartCoroutine(EnableCollision());
        _isMoving = true;
    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(_timeUntilPickup);
        GetComponent<BoxCollider>().enabled = true;
        _isMoving = false;
        _waitUntil = StartCoroutine(EnableCollision());
    }

    void Update()
    {
        if (_isMoving == true)
        {
            transform.Translate(transform.up * 0.15f * Time.deltaTime);
        }

    }
}
