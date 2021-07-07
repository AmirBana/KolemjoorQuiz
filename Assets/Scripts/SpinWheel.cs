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
	public CoinControl _CoinControl;

	[SerializeField] Text userScoreTxt;
	[SerializeField] Text userCoinTxt;

	[Space]
	[Header("Timing")]
	[SerializeField] double _WaitingForNextReward ;

	[Space]
	[Header("TestCanPlay")]
	[SerializeField] bool CanSpinForTime;
	public bool PlayerPrefsBool;


	double ElapsedSecound;
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

	void Start()
	{
		
		TimerOn = false;
		spinning = false;
		anglePerItem = 360 / _prize.Count;

	}
	void CheckReward()
	{
		DateTime CurrentTime = DateTime.Now;
		DateTime RewardClaimDateTime = DateTime.Parse(ObscuredPrefs.GetString("Reward_Claim_DataTime"));
		ElapsedSecound = (CurrentTime - RewardClaimDateTime).TotalSeconds;
		if (ElapsedSecound >= _WaitingForNextReward)
		{
			CanSpinForTime = true;
		}
		else
		{
			CanSpinForTime = false;
			ObscuredPrefs.SetBool("IsPlayed", false);


		}
	}

	void Update()
	{
		if (string.IsNullOrEmpty(ObscuredPrefs.GetString("Reward_Claim_DataTime")))
		{
			ObscuredPrefs.SetString("Reward_Claim_DataTime", DateTime.Now.ToString());
		}
		CheckReward();
		PlayerPrefsBool = ObscuredPrefs.GetBool("IsPlayed");
		if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				prePos = Input.GetTouch(0).position;
			}

			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				ObscuredPrefs.SetString("Reward_Claim_DataTime", DateTime.Now.ToString());
				curPos = Input.GetTouch(0).position;
				deltaPos = curPos.magnitude - prePos.magnitude;
				if (deltaPos > Sens || -deltaPos < Sens)
				{
					{
						spining();
						Debug.Log("Drag Done!");
					}

				}

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
			if (!spinning && CanSpinForTime && !ObscuredPrefs.GetBool("IsPlayed", false)/*!TimerOn && */)
			{
				print("Spining ....");
				ObscuredPrefs.SetBool("IsPlayed", true);
				randomTime = UnityEngine.Random.Range(1, 4);
				itemNumber = UnityEngine.Random.Range(0, _prize.Count);
				Debug.Log(itemNumber);
				float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);
				if (_prize[itemNumber].isCoin)
				{
					print("Priz Is : " + _prize[itemNumber].allPrize);
					CoinControl.ManageCoin(_prize[itemNumber].allPrize);
				}
				Invoke("UpdateCoin", 5 * randomTime);
				StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
				ObscuredPrefs.SetBool("IsPlayed", true);
			}
		}
		void UpdateCoin()
		{
			userScoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
			userCoinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
			print("Update");
		}
	}
}