using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameTimer gameTimer;

    private void OnEnable()
    {
        gameTimer.OnTimerUpdated += UpdateTimerValue;
    }

    private void OnDisable()
    {
        gameTimer.OnTimerUpdated -= UpdateTimerValue;
    }

    private void UpdateTimerValue(int value)
    {
        var mins = value / 60;
        var secs = value % 60;

        var s_mins = mins < 10 ? "0" + mins : ""+mins;
        var s_secs = secs < 10 ? "0" + secs : ""+secs;
        
        timerText.text = s_mins + ":" + s_secs;
    }
}
