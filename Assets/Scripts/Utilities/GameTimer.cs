using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] 
    private int timerStartValue = 120;
    
    private int _timerValue;
    private Coroutine _timerCoroutine;
    private bool _isTimerRunning;

    public event Action<int> OnTimerUpdated;

    private IEnumerator Timer(int timerStartVal)
    {
        _timerValue = timerStartValue;
        _isTimerRunning = true;

        while (_timerValue >= 0)
        {
            OnTimerUpdated?.Invoke(_timerValue);
            _timerValue--;
            yield return new WaitForSeconds(1);
        }

        _isTimerRunning = false;
    }

    public void StartTimer()
    {
        if (_isTimerRunning) return;

        _timerCoroutine = StartCoroutine(Timer(timerStartValue));
    }

    public void StopTimer()
    {
        if (!_isTimerRunning) return;

        _isTimerRunning = false;
        StopCoroutine(_timerCoroutine);
    }
}
