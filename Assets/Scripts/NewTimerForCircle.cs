using CodeStage.AntiCheat.Storage;
using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class NewTimerForCircle : MonoBehaviour
{

	public Button timerButton;

	public Text timeLabel;

	public string Time24H;

	private double tcounter;

	private TimeSpan eventEndTime;

	private TimeSpan currentTime;

	private TimeSpan _remainingTime;

	private string Timeformat;

	private bool timerSet;

	private bool countIsReady;

	private bool countIsReady2;

	public bool IsRedayChance;

	public string set24;
	private TimeSpan Span24;
	double countt;
	double t;

	void Start()
	{

		eventEndTime = TimeSpan.Parse(Time24H);
		countIsReady2 = false;
		StartCoroutine("CheckTime");
		Span24 = TimeSpan.Parse(set24);
		t = Span24.TotalSeconds;
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
		double countt = t - Time.deltaTime;
		t -= countt;
		//Debug.Log(countt - Time.deltaTime );
		TimeSpan tempc = TimeSpan.FromSeconds(t);
		string Timeformat2 = string.Format("{0:D2}:{1:D2}:{2:D2}", tempc.Hours, tempc.Minutes, tempc.Seconds);
		print(Timeformat2);
		//print(Timeformat2);

		if (timerSet && ObscuredPrefs.GetBool("IsPlayed"))
	


		{
			//this means the event as already started and players can click and join

			_remainingTime = eventEndTime.Subtract(currentTime);

				tcounter = _remainingTime.TotalMilliseconds;

				countIsReady2 = true;
				IsRedayChance = true;



			
			
		}




		if (countIsReady2) { startCountdown2(); }
		if (ObscuredPrefs.GetBool("IsPlayed"))
		{
			disableButton("You Turn Wheel ");
			startCountdown2();
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






	private void startCountdown2()

	{

		timerSet = false;

		tcounter -= Time.deltaTime * 1000;

		enableButton("شروع شد! زمان باقيمانده : " + GetRemainingTime(tcounter));



		if (tcounter <= 0)
		{

			countIsReady2 = false;
			ObscuredPrefs.SetBool("IsPlayed", true);
			//validateTime();
			//print("HEEEEEEEEEEEEEEEEEEEEEEEEEEy");

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