using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public FencedBreadsCounter corral;
    public TMP_Text text;
    private float startTime;
    private bool isRunning;
    
    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    void Awake()
    {
        StartTimer();
    }

    void Update()
    {
        if(corral.Counter >= 50)
        {
            StopTimer();
            ShowWinWindow();
        }

        if (isRunning)
        {
            float ellapsedTime = Time.time - startTime;
            text.text = $"{corral.Counter}/50\n{FormatTime(ellapsedTime)}";
        }
    }

    private void ShowWinWindow()
    {
        
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        return timeSpan.TotalHours >= 1
            ? timeSpan.ToString(@"hh\:mm\:ss")
            : timeSpan.ToString(@"mm\:ss");
    }
}
