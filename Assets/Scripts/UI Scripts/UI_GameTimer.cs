
using UnityEngine;
using TMPro;
using System;
using System.Timers;
public class UI_GameTimer : MonoBehaviour
{
    
    TextMeshProUGUI timerText;

    string convertedTime;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        Time.timeScale = 30f; //Timescale for Testing
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ConvertTimeToText();
    }

    private string ConvertTimeToText()
    {
        convertedTime = new string(TimeContainer.convertedHours.ToString() + "D " + TimeContainer.convertedMinutes.ToString("00") + ":" + TimeContainer.convertedSeconds.ToString("00"));

        return convertedTime;
    }
}
