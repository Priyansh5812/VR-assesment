using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "QuestionBank" , menuName = "Scriptable Object/QuestionBank")]
public class QuestionBank : ScriptableObject
{
    public QuestionData[] questionData;
}

[System.Serializable]
public struct QuestionData
{
    public string question;
    public string[] answers;
    public int answerIndex;
}