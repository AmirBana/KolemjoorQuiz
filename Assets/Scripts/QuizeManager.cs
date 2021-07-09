using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class QuizeManager : MonoBehaviour
{
	//public List<Questions> unAnswered = new List<Questions>();
	//private List<Questions> answereds = new List<Questions>();
	public QuizData quizData;
	[Range(1, 5)] public int currentHardness;
	private int isShowed;
	public Text questionTxt;
	public Button[] answerBtns;
	private Text[] answerBtnsTxt;
	public float changeQuestionTime;
	public Text scoreTxt;
	public Text coinTxt;
	public GameObject plusTxt;
	public GameObject minusTxt;
	public int scoreForCorrect;
	public int scoreForWrong;
	public Sprite imgWrong;
	public Sprite imgCorrect;
	public Sprite imgAnswer;
	private bool isTimeBuyed;
	[SerializeField] float startTime = 12f;
	[SerializeField] Slider TimerSlider;
	[SerializeField] Text timerTxt;
	[SerializeField] Button timerBtn;
	[SerializeField] GameObject goNext;
	//Helps
	public Button delete2Answers;
	public Button tryAgain;
	public GameObject coinWarning;
	private bool isTried = false;
	//finished
	public Text finalScoreTxt;
	public Text finalCoin;
	public Text coinEarned;
	public Text finalCorrects;
	public GameObject finishedCanvas;
	public GameObject questionCanvas;
	private int correct = 0;
	private bool quizeTimeStart = false;

	string NameStars;
	public int MinTrue;
	public int NormTrue;
	public int MaxTrue;
	public GameObject[] Stars;
	int NumStars = 0;
	float timer;
	bool isStarted = false;
	DateTime pauseDateTime;

	// Start is called before the first frame update
	void Awake()
	{
		answerBtnsTxt = new Text[answerBtns.Length];
		for (int i = 0; i < answerBtns.Length; i++)
		{
			answerBtnsTxt[i] = answerBtns[i].gameObject.transform.GetChild(0).GetComponent<Text>();
		}
	}
	private void Start()
	{
		isStarted = true;
		isShowed = 0;
		NameStars = "stars" + SceneManager.GetActiveScene().name;
		print("First Stars: " + NameStars);
		for (int j = 0; j < Stars.Length; j++)
		{
			Stars[j].SetActive(false);
		}
	}
	private void OnApplicationPause(bool pause)
	{
		if (isStarted)
		{
			if (pause)
			{
				pauseDateTime = DateTime.Now;
			}
			else
			{
				timer -= Mathf.Abs((int)(DateTime.Now - pauseDateTime).TotalSeconds);
			}
		}

	}
	void OnEnable()
	{
		SelectQuestion();
		SetScoreTxt();
		SetCoinTxt();
		isTimeBuyed = false;
	}
	private IEnumerator Timer()
	{
		timer = startTime;
		do
		{
			timer -= Time.deltaTime;
			TimerSlider.value = timer / startTime;
			timerTxt.text = (int)(timer + 1f) + "";
			yield return null;
		} while (timer > 0f);
		SelectQuestion();

	}
	private void BuyExtraTime()
	{
		if (ObscuredPrefs.GetInt("Coin") >= 30)
			ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") - 30);
		else
		{
			StartCoroutine(CoinWarning());
			return;
		}
		SetCoinTxt();
		StopCoroutine("Timer");
		StartCoroutine("Timer");
		isTimeBuyed = true;
		timerBtn.interactable = false;
	}
	private void SetScoreTxt()
	{
		scoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
	}
	private void SetCoinTxt()
	{
		coinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
	}
	private void SetActiveOff()
	{
		plusTxt.SetActive(false);
		minusTxt.SetActive(false);
	}
	private void SelectQuestion()
	{
		IntractableAnswerBtns(true);
		delete2Answers.interactable = true;
		SetActiveOff();
		if (isShowed == 10)
		{
			StopCoroutine("Timer");
			FinishGame();
			return;
		}
		StartCoroutine("Timer");
		int random;
		int countQ = 0;
		do
		{
			random = Random.Range(0, quizData.unAnsweredQ.Count);
			countQ++;
		} while (quizData.unAnsweredQ[random].hardness != currentHardness && countQ < quizData.unAnsweredQ.Count);
		questionTxt.text = quizData.unAnsweredQ[random].questionTxt;
		for (int i = 0; i < answerBtns.Length; i++)
		{
			answerBtnsTxt[i].text = quizData.unAnsweredQ[random].answers[i].answerTxt;
			answerBtns[i].GetComponent<Image>().color = Color.white;
			Button currentBtn = answerBtns[i];
			delete2Answers.onClick.RemoveAllListeners();
			currentBtn.onClick.RemoveAllListeners();
			timerBtn.onClick.RemoveAllListeners();
			if (quizData.unAnsweredQ[random].answers[i].isCorrect)
			{

				answerBtns[i].onClick.AddListener(() => StartCoroutine(CorrectAnswer(currentBtn)));


			}
			else
			{
				answerBtns[i].onClick.AddListener(() => WrongAnswer(currentBtn));
			}
		}
		quizData.answeredQ.Add(quizData.unAnsweredQ[random]);
		isShowed++;
		quizData.unAnsweredQ.RemoveAt(random);
		timerBtn.interactable = true;
		timerBtn.onClick.AddListener(() => BuyExtraTime());
		if (delete2Answers.interactable)
			delete2Answers.onClick.AddListener(() => Delete2Choses());
	}
	void Delete2Choses()
	{
		if (ObscuredPrefs.GetInt("Coin") >= 40)
		{
			ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") - 40);
			SetCoinTxt();
		}
		else
		{
			StartCoroutine(CoinWarning());
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			int rand;
			do
			{
				rand = Random.Range(0, answerBtns.Length);
			} while (answerBtns[rand].interactable == false
			  || quizData.answeredQ[quizData.answeredQ.Count - 1].answers[rand].isCorrect);
			answerBtns[rand].interactable = false;
			answerBtns[rand].GetComponent<Image>().color = new Color32(200, 200, 200, 255);
			string txt = answerBtnsTxt[rand].text;
		}
		delete2Answers.interactable = false;

	}
	private void ShowScoreChange(Vector3 pos, GameObject scoreTxt)
	{
		scoreTxt.transform.position = pos;
		scoreTxt.SetActive(true);
	}
	IEnumerator CorrectAnswer(Button currentBtn)
	{
		delete2Answers.onClick.RemoveAllListeners();
		StopCoroutine("Timer");
		ShowScoreChange(currentBtn.transform.position, plusTxt);
		ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") + scoreForCorrect);
		correct++;
		SetScoreTxt();
		Image btnImg = currentBtn.GetComponent<Image>();
		btnImg.sprite = imgCorrect;
		IntractableAnswerBtns(false);
		yield return new WaitForSeconds(changeQuestionTime);
		btnImg.sprite = imgAnswer;
		SelectQuestion();
	}
	private void WrongAnswer(Button currentBtn)
	{
		delete2Answers.onClick.RemoveAllListeners();
		StopCoroutine("Timer");
		Image btnImg = currentBtn.GetComponent<Image>();
		btnImg.sprite = imgWrong;
		IntractableAnswerBtns(false);
		goNext.GetComponent<Button>().onClick.RemoveAllListeners();
		goNext.SetActive(true);
		timerBtn.interactable = false;
		goNext.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(GoNextQuestion(btnImg, currentBtn)));
		tryAgain.interactable = true;

	}
	IEnumerator GoNextQuestion(Image btnImg, Button currentBtn)
	{
		tryAgain.interactable = false;
		if (ObscuredPrefs.GetInt("Score") < 10)
			ObscuredPrefs.SetInt("Score", 0);
		else
		{
			ShowScoreChange(currentBtn.transform.position, minusTxt);
			ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") - scoreForWrong);
		}
		SetScoreTxt();
		yield return new WaitForSeconds(changeQuestionTime);
		btnImg.sprite = imgAnswer;
		goNext.SetActive(false);
		SelectQuestion();

	}
	public void TryAgain()
	{
		if (ObscuredPrefs.GetInt("Coin") >= 40)
		{
			ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") - 40);
			SetCoinTxt();
		}
		else
		{
			StartCoroutine(CoinWarning());
			return;
		}
		//everything comes here
		timerBtn.interactable = true;
		goNext.SetActive(false);
		IntractableAnswerBtns(true);
		StartCoroutine("Timer");
		for (int i = 0; i < answerBtns.Length; i++)
		{
			answerBtns[i].GetComponent<Image>().sprite = imgAnswer;
		}
		tryAgain.interactable = false;
		isTried = true;
	}
	void IntractableAnswerBtns(bool i)
	{
		foreach (Button btn in answerBtns)
			btn.interactable = i;
	}
	IEnumerator CoinWarning()
	{
		coinWarning.SetActive(true);
		yield return new WaitForSeconds(1);
		coinWarning.SetActive(false);
	}
	private void CalculateCoin()
	{
		if (correct <= 4 && correct >= 0)
		{
			//1 star
			//should wait 10 minute and 0 coin
			UpdateMaxLevel();
			NumStars = 1;
			Stars[0].SetActive(true);
			print("1 Star!");
			coinEarned.text = "" + 0;
			quizeTimeStart = true;
			if (WorldTimeAPI.Instance.IsTimeLoaded)
				ObscuredPrefs.SetString("IsLost", WorldTimeAPI.Instance.GetCurrentDateTime().AddMinutes(10).ToString());
			else
				ObscuredPrefs.SetString("IsLost", DateTime.Now.AddMinutes(10).ToString());

		}
		else if (correct <= 8)
		{
			//2 stars
			UpdateMaxLevel();
			NumStars = 2;
			Stars[0].SetActive(true);
			Stars[1].SetActive(true);
			print("2 Star!");
			ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 15);
			coinEarned.text = "" + 15;
		}
		else if (correct == 9)
		{
			UpdateMaxLevel();
			NumStars = 3;
			Stars[0].SetActive(true);
			Stars[1].SetActive(true);
			Stars[2].SetActive(true);
			print("3 Star!");
			//3 stars
			ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 25);
			coinEarned.text = "" + 25;
		}
		else if (correct == 10)
		{
			UpdateMaxLevel();
			NumStars = 3;
			Stars[0].SetActive(true);
			Stars[1].SetActive(true);
			Stars[2].SetActive(true);
			print("3 Star!");
			if (UsedHelp())
			{
				ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 25);
				coinEarned.text = "" + 25;
			}

			else
			{
				UpdateMaxLevel();
				ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 40);
				coinEarned.text = "" + 40;
			}

		}
		print("-------------------");
		print(NumStars);
		print(ObscuredPrefs.GetInt(NameStars, 0));
		print(NameStars);
		print("-------------------");
		if (NumStars >= ObscuredPrefs.GetInt(NameStars, 0))
		{
			ObscuredPrefs.SetInt(NameStars, NumStars);

		}
		else
		{
			print("no equal num stars and get stars");
		}
	}
	private bool UsedHelp()
	{
		if (!delete2Answers.interactable || isTimeBuyed || isTried)
			return true;
		else
			return false;
	}
	private void FinishGame()
	{
		questionCanvas.SetActive(false);
		finishedCanvas.SetActive(true);
		finalScoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
		finalCorrects.text = "" + correct;
		CalculateCoin();
		finalCoin.text = "" + ObscuredPrefs.GetInt("Coin");
		delete2Answers.interactable = true;


	}

	public void Restart()
	{
		if (quizeTimeStart)
		{
			if (ObscuredPrefs.GetInt("LostHeart") < 5)
			{
				ObscuredPrefs.SetInt("LostHeart", ObscuredPrefs.GetInt("LostHeart") + 1);
				Debug.Log("after: " + ObscuredPrefs.GetString("LostHeartDate"));
				if (ObscuredPrefs.GetString("LostHeartDate") == "")
				{
					if (WorldTimeAPI.Instance.IsTimeLoaded)
					{
						ObscuredPrefs.SetString("LostHeartDate", WorldTimeAPI.Instance.GetCurrentDateTime().AddDays(1).ToString());
						Debug.Log("Time from api: " + ObscuredPrefs.GetString("LostHeartDate"));
					}
					else
					{
						ObscuredPrefs.SetString("LostHeartDate", DateTime.Now.AddDays(1).ToString());
						Debug.Log("Local Time: " + ObscuredPrefs.GetString("LostHeartDate"));
					}
				}
				quizeTimeStart = false;
				//unAnswered = new List<Questions>(answereds);
				//answereds.Clear();
				correct = 0;
				ObscuredPrefs.SetString("IsLost", "");
				questionCanvas.SetActive(true);
				finishedCanvas.SetActive(false);
			}
			else
			{
				print("u cant restart now");
			}
		}
		else
		{
			quizeTimeStart = false;
			//unAnswered = new List<Questions>(answereds);
			//answereds.Clear();
			correct = 0;
			questionCanvas.SetActive(true);
			finishedCanvas.SetActive(false);
		}
	}
	private void UpdateMaxLevel()
	{
		print("build: " + (SceneManager.GetActiveScene().buildIndex - 3) + "Max: " + ObscuredPrefs.GetInt("MaxLevel"));
		if (SceneManager.GetActiveScene().buildIndex - 3 == ObscuredPrefs.GetInt("MaxLevel"))
		{
			ObscuredPrefs.SetInt("MaxLevel", ObscuredPrefs.GetInt("MaxLevel") + 1);

		}

	}

	//developer
	//Update method is related to developer and can be deleted for final build
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			ObscuredPrefs.SetInt("Score", 0);
			Debug.Log("Score Sets to 0");
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			ObscuredPrefs.SetInt("Coin", 0);
			Debug.Log("Coins sets to 0");
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 10);
			Debug.Log("Added 10 coins");
			SetCoinTxt();
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			ObscuredPrefs.SetInt("LostHeart", 0);
			Debug.Log("full hearts");
		}

	}
}