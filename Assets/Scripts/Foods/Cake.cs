using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : Food
{
    private Coroutine _enableCollisionCoroutine;
    private bool _isRising;
    private float _timeUntilPickup = 5f;

    void Start()
    {
        Value = GameConstants.CakeValue;
        GetComponent<BoxCollider>().enabled = false;
        _enableCollisionCoroutine = StartCoroutine(EnableCollisionAfterDelay());
        _isRising = true;
    }

    private IEnumerator EnableCollisionAfterDelay()
    {
        yield return new WaitForSeconds(_timeUntilPickup);
        GetComponent<BoxCollider>().enabled = true;
        _isRising = false;
    }

    void Update()
    {
        if (_isRising)
        {
            RiseOverTime();
        }
    }

    private void RiseOverTime()
    {
        transform.Translate(Vector3.up * 0.15f * Time.deltaTime);
    }
}
