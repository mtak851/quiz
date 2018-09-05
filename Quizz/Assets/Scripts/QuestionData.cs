using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuestionData
{
    public string questionText; //for the question text
    public AnswerData[] answers; // each question will hold the number of answer that are associated to!
}
