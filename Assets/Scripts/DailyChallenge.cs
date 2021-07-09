using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using Random = UnityEngine.Random;

public class DailyChallenge : MonoBehaviour
{
	//public List<Questions> unAnswered = new List<Questions>();
	//private List<Questions> answereds = new List<Questions>();
	public QuizData quizData;
	[Range(1, 5)] public int hardness;
	private int questionShown;
	public Text questionTxt;
	public Button[] answerBtns;
	private Text[] answerBtnsTxt;
	public float changeQuestionTime;
	public Text scoreTxt;
	public Text coinTxt;
	public GameObject plusTxt;
	public GameObject minusTxt;
	public int scoreForCorrect = 3;
	public int scoreForWrong = 2;
	public int questionCoin = 15;
	public Sprite imgWrong;
	public Sprite imgCorrect;
	public Sprite imgAnswer;
	[SerializeField] float startTime = 12f;
	private bool paused = false;
	[SerializeField] Slider TimerSlider;
	[SerializeField] Text timerTxt;
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
	bool isStarted = false;
	float timer;
	DateTime pausedDateTime;

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
		StartCoroutine("Timer");
		questionShown = 1;
	}
	private void OnApplicationPause(bool pause)
	{
		if (isStarted)
		{
			if (pause)
			{
				pausedDateTime = DateTime.Now;
			}
			else
			{
				timer -= Mathf.Abs((int)(DateTime.Now - pausedDateTime).TotalSeconds);
			}
		}
	}
	void OnEnable()
	{
		SelectQuestion();
		SetScoreTxt();
		SetCoinTxt();
	}
	private IEnumerator Timer()
	{
		timer = startTime;
		do
		{
			while (paused)
			{
				yield return null;
			}
			timer -= Time.deltaTime;
			TimerSlider.value = timer / startTime;
			timerTxt.text = (int)(timer + 1f) + "";
			yield return null;
		} while (timer > 0f);
		FinishGame();
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
		SetActiveOff();
		if (questionShown == 5)
		{
			//StopCoroutine("Timer");
			FinishGame();
			return;
		}
		//StartCoroutine("Timer");
		int random;
		do
		{
			random = Random.Range(0, quizData.unAnsweredQ.Count);
		} while (quizData.unAnsweredQ[random].hardness != hardness);
		questionTxt.text = quizData.unAnsweredQ[random].questionTxt;
		for (int i = 0; i < answerBtns.Length; i++)
		{
			answerBtnsTxt[i].text = quizData.unAnsweredQ[random].answers[i].answerTxt;
			answerBtns[i].GetComponent<Image>().color = Color.white;
			Button currentBtn = answerBtns[i];
			delete2Answers.onClick.RemoveAllListeners();
			currentBtn.onClick.RemoveAllListeners();
			if (quizData.unAnsweredQ[random].answers[i].isCorrect)
			{

				answerBtns[i].onClick.AddListener(() => StartCoroutine(CorrectAnswer(currentBtn)));
			}
			else
			{
				answerBtns[i].onClick.AddListener(() => StartCoroutine(WrongAnswer(currentBtn)));
			}
		}
		quizData.answeredQ.Add(quizData.unAnsweredQ[random]);
		questionShown++;
		quizData.unAnsweredQ.RemoveAt(random);
		paused = false;
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
		paused = true;
		delete2Answers.onClick.RemoveAllListeners();
		//StopCoroutine("Timer");
		ShowScoreChange(currentBtn.transform.position, plusTxt);
		ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") + scoreForCorrect);
		ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + questionCoin);
		coinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
		correct++;
		SetScoreTxt();
		Image btnImg = currentBtn.GetComponent<Image>();
		btnImg.sprite = imgCorrect;
		IntractableAnswerBtns(false);
		yield return new WaitForSeconds(changeQuestionTime);
		btnImg.sprite = imgAnswer;
		SelectQuestion();
	}
	/*private void*/
	IEnumerator WrongAnswer(Button currentBtn)
	{
		paused = true;
		delete2Answers.onClick.RemoveAllListeners();
		//StopCoroutine("Timer");
		Image btnImg = currentBtn.GetComponent<Image>();
		btnImg.sprite = imgWrong;
		IntractableAnswerBtns(false);
		//goNext.GetComponent<Button>().onClick.RemoveAllListeners();
		//goNext.SetActive(true);
		//goNext.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(GoNextQuestion(btnImg, currentBtn)));
		//if(!isTried)
		//tryAgain.interactable = true;
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
		//goNext.SetActive(false);
		SelectQuestion();

	}
	/*IEnumerator GoNextQuestion(Image btnImg, Button currentBtn) {
        if(tryAgain.interactable == true)
            tryAgain.interactable = false;
        if(ObscuredPrefs.GetInt("Score") < 10)
            ObscuredPrefs.SetInt("Score", 0);
        else {
            ShowScoreChange(currentBtn.transform.position, minusTxt);
            ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") - scoreForWrong);
        }
        SetScoreTxt();
        yield return new WaitForSeconds(changeQuestionTime);
        btnImg.sprite = imgAnswer;
        goNext.SetActive(false);
        SelectQuestion();

    }*/
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
	/*private void CalculateCoin() {
        if(correct <= 4 && correct >= 0) {
            //1 star
            //should wait 10 minute and 0 coin
            coinEarned.text = "" + 0;
        }
        else if(correct <= 8) {
            //2 stars
            ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 15);
            coinEarned.text = "" + 15;
        }
        else if(correct == 9) {
            //3 stars
            ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 25);
            coinEarned.text = "" + 25;
        }
        else if(correct == 10) {
            if(UsedHelp()) {
                ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 25);
                coinEarned.text = "" + 25;
            }

            else {
                ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + 40);
                coinEarned.text = "" + 40;
            }
        }
    }*/
	private bool UsedHelp()
	{
		if (!delete2Answers.interactable || isTried)
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
		//CalculateCoin();
		coinEarned.text = "" + (correct * 15);
		finalCoin.text = "" + ObscuredPrefs.GetInt("Coin");
		delete2Answers.interactable = true;
	}
	public void Restart()
	{
		//unAnswered = new List<Questions>(answereds);
		//answereds.Clear();
		correct = 0;
		questionCanvas.SetActive(true);
		finishedCanvas.SetActive(false);
	}
	private void Update()
	{//developer
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

	}
}