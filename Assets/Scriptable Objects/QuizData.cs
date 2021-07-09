using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "Questions")]
public class QuizData : ScriptableObject
{
	public List<Question> unAnsweredQ = new List<Question>();
	public List<Question> answeredQ = new List<Question>();
}

[System.Serializable]
public class Question
{
	public string questionTxt;
	public Answer[] answers;
	[Range(1, 5)] public int hardness;
}
[System.Serializable]
public class Answer
{
	public string answerTxt;
	public bool isCorrect = false;
}