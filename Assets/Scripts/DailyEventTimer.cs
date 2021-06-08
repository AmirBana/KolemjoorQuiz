using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;

public class DailyEventTimer : MonoBehaviour
{
    public Button timerButton;
    public Text timeLabel;
    public string StartTime;
    public string EndTime;
    private double tcounter;
    private TimeSpan eventStartTime;
    private TimeSpan eventEndTime;
    private TimeSpan currentTime;
    private TimeSpan _remainingTime;
    private string Timeformat;
    private bool timerSet;
    private bool countIsReady;
    private bool countIsReady2;
    public bool ReadyToRotate;
    public bool IsPlayed;
    void Start()
    {
        eventStartTime = TimeSpan.Parse(StartTime);
        eventEndTime = TimeSpan.Parse(EndTime);
        StartCoroutine("CheckTime");
    }


    private IEnumerator CheckTime()
    {
        Debug.Log("==> Checking the time");
        timeLabel.text = "بررسی اتصال به اینترنت!";
        yield return StartCoroutine(
            TimeManager.sharedInstance.getTime()
        );
        updateTime();
        Debug.Log("بررسی اینترنت انجام شد");

    }



    private void updateTime()
    {
        currentTime = TimeSpan.Parse(TimeManager.sharedInstance.getCurrentTimeNow());
        timerSet = true;
    }

    

    void Update()
    {
        IsPlayed = ObscuredPrefs.GetBool("IsPlayed");print(IsPlayed);
        if (timerSet)
        {

            if (ObscuredPrefs.GetBool("IsPlayed"))
            {
                disableButton("شما  چالش روزانه را بازی کرده اید!");
            }


            if (currentTime >= eventStartTime && currentTime <= eventEndTime)
            {//this means the event as already started and players can click and join
                _remainingTime = eventEndTime.Subtract(currentTime);
                tcounter = _remainingTime.TotalMilliseconds;
                ObscuredPrefs.SetBool("ReadyToRotate",true);
                countIsReady2 = true;
              
            }
            else if (currentTime < eventStartTime )
            {//this means the event had not started yet for today
                _remainingTime = eventStartTime.Subtract(currentTime);
                tcounter = _remainingTime.TotalMilliseconds;
                //ObscuredPrefs.SetBool("ReadyToRotate", false);
                countIsReady = true;

            }
            if (currentTime >= eventEndTime)
            {
                //ObscuredPrefs.SetBool("IsPlayed", false);

            }
            else
            {//the event as already passed
                disableButton("چالش روز به اتمام رسیده است!");
                ObscuredPrefs.SetBool("IsPlayed", false);
            }
        }

        if (countIsReady ) { startCountdown(); }
        if (countIsReady2 ) { startCountdown2(); }
    }

    //setup the time format string
    public string GetRemainingTime(double x)
    {
        TimeSpan tempB = TimeSpan.FromMilliseconds(x);
        Timeformat = string.Format("{0:D2}:{1:D2}:{2:D2}", tempB.Hours, tempB.Minutes, tempB.Seconds);
        return Timeformat;
    }

    public void Played()
    {
        ObscuredPrefs.SetBool("IsPlayed", true);
    }

    private void startCountdown()
    {
        timerSet = false;
        tcounter -= Time.deltaTime * 1000;
        disableButton("چالش روزانه به زودی شروع میشود : " + GetRemainingTime(tcounter));

        if (tcounter <= 0)
        {
            countIsReady = false;
            validateTime();
        }
    }

    private void startCountdown2()
    {
        timerSet = false;
        tcounter -= Time.deltaTime * 1000;
        if (ObscuredPrefs.GetBool("ReadyToRotate") &&!ObscuredPrefs.GetBool("IsPlayed"))
        {
            enableButton("چالش روزانه شروع شد! تا پايان : " + GetRemainingTime(tcounter));

        }



        if (tcounter <= 0)
        {
            countIsReady2 = false;
            validateTime();
        }
    }

    //enable button function
    private void enableButton(string x)
    {
        timerButton.interactable = true;
        timeLabel.text = x;
    }


    //disable button function
    public void disableButton(string x)
    {
        timerButton.interactable = false;
        timeLabel.text = x;
    }

    //validator
    private void validateTime()
    {
        Debug.Log("در حال ارزیابی زمان!");
        StartCoroutine("CheckTime");
    }

}