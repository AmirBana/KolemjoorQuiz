using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;

public class SpinWheel : MonoBehaviour
{
	public List<priz> _prize;
	public List<AnimationCurve> animationCurves;
	public bool TimerOn;
	public float Timer;
	public GameObject TimeManager = null;
	private bool spinning;	
	private float anglePerItem;	
	private int randomTime;
	private int itemNumber;
	public Text timeText;
	float timeToDisplay = 86400f;
	float Secound;
	private TimeSpan timeSpan;
	bool  daily;
	public CoinControl _CoinControl;

	public Text userScoreTxt;
	public Text userCoinTxt;


	Vector2 curPos;
	Vector2 prePos;
	float deltaPos;
	public float Sens = 3f;
	/*
	void DisplayTime()
    {


			
		TimeSpan t = TimeSpan.FromSeconds(timeToDisplay);
		//t = t.Subtract(ts);
		timeToDisplay -= Time.deltaTime * 1.32f;

		//float minutes = Mathf.FloorToInt(timeToDisplay / 60 %60);
		//float seconds = Mathf.FloorToInt(timeToDisplay % 60);
		//float Hour = Mathf.FloorToInt(timeToDisplay / 60 /60);
		//print(t);
		//timeText.text = string.Format("{0:00}:{1:00}:{2:00}",Hour, minutes, seconds);
		timeText.text = t.Hours + ":" + t.Minutes + ":" + t.Seconds ;
		 
	}
	*/
	private void OnApplicationQuit()
	{
		DateTime QuitTime = DateTime.Now;
		ObscuredPrefs.SetString("_QuitTime", QuitTime.ToString());
	}
	void Start()
	{
		string LoadTime = ObscuredPrefs.GetString("_QuitTime");
		if (!LoadTime.Equals(""))
		{
			DateTime LoadDateTime = DateTime.Parse(LoadTime);
			DateTime NowTime = DateTime.Now;
			if (NowTime > LoadDateTime)
			{
				TimeSpan timeSpan = NowTime - LoadDateTime;
				Secound = (float)timeSpan.TotalSeconds;
				//timeToDisplay -= Secound;
				//Debug.Log(Secound);
				//DisplayTime();
			}

		}
		StartCoroutine("plusTime");

		TimerOn = false;
		spinning = false;
		anglePerItem = 360 / _prize.Count;

	}
	IEnumerator plusTime()
	{
		yield return new WaitForSeconds(1);
		timeToDisplay -= 1;
	}

	void Update()
		{
		if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				prePos = Input.GetTouch(0).position;
			}

			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				daily = GameObject.Find("ChanceCanvas").GetComponent<DailyEventTimer>().IsRedayChance;
				curPos = Input.GetTouch(0).position;
				deltaPos = curPos.magnitude - prePos.magnitude;
				if (deltaPos > 0 && deltaPos > Sens)
				{
					if (Mathf.Abs(curPos.x - prePos.x) > Mathf.Abs(curPos.y - prePos.y)  )                                    // right
					{
						spining();
						Debug.Log("Right");
					}
					else
					{
						spining();                                                                                   //up
						Debug.Log("Up");


					}

				}
				else if (deltaPos < 0 && Mathf.Abs(deltaPos) > Sens)
				{
					if (Mathf.Abs(curPos.x - prePos.x) > Mathf.Abs(curPos.y - prePos.y))                                  //left
					{
						spining();
						Debug.Log("Left");



					}
					else
					{
						spining();
						Debug.Log("Down");                                                                                      //down
						Camera.main.transform.position += new Vector3(0, -1, 0) * Time.deltaTime * 10f;

					}

				}

			}
		}


		if (TimerOn)
			{
			//DisplayTime();
			}
			if (timeToDisplay < 0)
			{
			TimerOn = false;
			}
		}

		IEnumerator SpinTheWheel(float time, float maxAngle)
		{
			spinning = true;
			TimerOn = true;
			float timer = 0.0f;
			float startAngle = transform.eulerAngles.z;
			maxAngle = maxAngle + 180 - startAngle;

			int animationCurveNumber = UnityEngine.Random.Range(0, animationCurves.Count);
			Debug.Log("Animation Curve No. : " + animationCurveNumber);

			while (timer < time)
			{
				//to calculate rotation
				float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
				transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
				timer += Time.deltaTime;
				yield return 0;
			}

			transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
			spinning = false;

			Debug.Log("Prize: " + _prize[itemNumber].allPrize + " Is Coin:" + _prize[itemNumber].isCoin);
		}


	void spining()
    {
		if (!spinning  && daily && !ObscuredPrefs.GetBool("IsPlayed")/*!TimerOn && */)
		{
			print("spiiiin");
			randomTime = UnityEngine.Random.Range(1, 4);
			itemNumber = UnityEngine.Random.Range(0, _prize.Count);
			Debug.Log(itemNumber);
			float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
            if (_prize[itemNumber].isCoin)
            {
				print( "Priiiiiz: "+_prize[itemNumber].allPrize);
				CoinControl.ManageCoin(_prize[itemNumber].allPrize);
			}
			userScoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
			userCoinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
			StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
			ObscuredPrefs.SetBool("IsPlayed", true);
		}
	}
	} 