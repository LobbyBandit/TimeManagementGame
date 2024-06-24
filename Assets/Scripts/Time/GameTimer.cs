using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [HideInInspector]
    public float countDownTimer;
    bool _timeAssigned;

    public UnityEvent OnTimerZero;


    public float _displaySeconds;
    public float _displayMinutes;
    public float _displayMinutesInSeconds;
    public float _displayHours;

    // Start is called before the first frame update
    void Start()
    {
        if (_timeAssigned == false)
        {
            Debug.Log("No value was assigned to GameTimer");
            enabled = false;
        }       
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
        ConvertTime(); // 1h is Represented in steps of 24 
        UpdateTimeConverter();
        //Debug.Log(countDownTimer);
    }

    void CountDown()
    {
        countDownTimer -= Time.deltaTime;

        if (countDownTimer < 0)
            countDownTimer = 0;
    }

    public void SetInitialTimer(float timer)
    {
        countDownTimer = timer;
        _timeAssigned = true;
    }

    public void ConvertTime()
    {
        float Hours = Mathf.Floor(countDownTimer / 60 / 60);

        float Minutes = (countDownTimer - (Hours * 60 * 60)) / 150;

        float Seconds = countDownTimer - (Hours * 60 * 60) - (Mathf.Floor(Minutes) * 150);

        float convertedSeconds = Seconds / 2.5f;

        //Debug.Log(Mathf.Floor(Hours) + "Hours   " + Mathf.Floor(Minutes) + "Minutes   " + Mathf.Floor(Seconds) + "Seconds");

        _displaySeconds = Mathf.Floor(convertedSeconds);
        _displayMinutes = Mathf.Floor(Minutes);
        _displayMinutesInSeconds = Minutes;
        _displayHours = Hours;
    }

    void UpdateTimeConverter()
    {
        TimeContainer.convertedHours = _displayHours;
        TimeContainer.convertedMinutes = _displayMinutes;
        TimeContainer.convertedMinutesInSeconds = _displayMinutesInSeconds;
        TimeContainer.convertedSeconds = _displaySeconds;
    }

}
