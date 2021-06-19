using CodeStage.AntiCheat.Storage;
using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



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

	public bool IsRedayChance;





	void Start()
	{

		eventStartTime = TimeSpan.Parse(StartTime);

		eventEndTime = TimeSpan.Parse(EndTime);

		StartCoroutine("CheckTime");

	}





	private IEnumerator CheckTime()

	{

		Debug.Log("==> Checking the time");

		timeLabel.text = "در حال اعتبار سنجی زمان";

		yield return StartCoroutine(

			TimeManager.sharedInstance.getTime()

		);

		updateTime();

		Debug.Log("==> Time check complete!");



	}







	private void updateTime()

	{

		currentTime = TimeSpan.Parse(TimeManager.sharedInstance.getCurrentTimeNow());

		timerSet = true;

	}







	void Update()

	{

		if (timerSet)

		{

			if (currentTime >= eventStartTime && currentTime <= eventEndTime)

			{//this means the event as already started and players can click and join

				_remainingTime = eventEndTime.Subtract(currentTime);

				tcounter = _remainingTime.TotalMilliseconds;

				countIsReady2 = true;
				IsRedayChance = true;



			}
			else if (currentTime < eventStartTime)

			{//this means the event had not started yet for today

				_remainingTime = eventStartTime.Subtract(currentTime);

				tcounter = _remainingTime.TotalMilliseconds;

				countIsReady = true;
				IsRedayChance = false;
				ObscuredPrefs.SetBool("IsPlayed", false);


			}
			else

			{//the event as already passed

				disableButton("به پایان رسید! منتظر شروع مجدد باشيد!");

			}

		}




		if (countIsReady) { startCountdown(); }

		if (countIsReady2) { startCountdown2(); }
        if (ObscuredPrefs.GetBool("IsPlayed"))
        {
			disableButton("You Turn Wheel ");

			//Invoke("startCountdown", 4f);
		}
       

	}



	//setup the time format string

	public string GetRemainingTime(double x)

	{

		TimeSpan tempB = TimeSpan.FromMilliseconds(x);

		Timeformat = string.Format("{0:D2}:{1:D2}:{2:D2}", tempB.Hours, tempB.Minutes, tempB.Seconds);

		return Timeformat;

	}





	private void startCountdown()

	{

		timerSet = false;

		tcounter -= Time.deltaTime * 1000;

		disableButton("به زودی : " + GetRemainingTime(tcounter));



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

		enableButton("شروع شد! زمان باقيمانده : " + GetRemainingTime(tcounter));



		if (tcounter <= 0)
		{

			countIsReady2 = false;

			validateTime();

		}

	}



	//enable button function

	private void enableButton(string x)

	{

		//timerButton.interactable = true;
		IsRedayChance = true;
		timeLabel.text = x;

	}





	//disable button function

	private void disableButton(string x)

	{

		//timerButton.interactable = false;
		IsRedayChance = false;
		timeLabel.text = x;

	}



	//validator

	private void validateTime()

	{

		Debug.Log("==> Validating time to make sure no speed hack!");

		StartCoroutine("CheckTime");

	}



}