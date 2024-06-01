using System;
using System.Collections;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    public bool canIGo;
    
    private Coroutine _testCoroutine;

    private void Start()
    {
        _testCoroutine = StartCoroutine(TestIEnumerator());
    }

    private IEnumerator TestIEnumerator()
    {
        Debug.Log("Waiting until I am allowed!");
        yield return new WaitUntil(()=> canIGo);
        Debug.Log("Here I go!");
    }
}
