using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour //populate to display the answers which currently are not going to be clickable!
{
    
    public Text questionDisplayText;
    public Text scoreDisplayText;
    public Text timeRemainingDisplayText;

    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool; //array of the question data!

    private bool isRoundActive = false; //Is the game going?!
    private float timeRemaining; //the amount of time ledt in a round!
    private int questionIndex; 
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        dataController = FindObjectOfType<DataController>(); //get the store reference of the data controller! 
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;
        timeRemaining = currentRoundData.timeLimitInSeconds;
        UpdateTimeRemainingDisplay();

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;

    }

    private void ShowQuestion()  //clear out any existing answer button and show questions
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionDisplayText.text = questionData.questionText;

        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();//get me another button which is not currently used 
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent); 

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]); //pass to the answer 
        }
    }

    private void RemoveAnswerButtons() //remove the existing aswer button
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]); //send it back ro the object pool
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Score: " + playerScore.ToString();
        }

        if (questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestion();
        }
        else
        {
            EndRound();
        }

    }

    public void EndRound()
    {
        isRoundActive = false;
        dataController.SubmitNewPlayerScore (playerScore);
        highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScreen"); //always loading back to the menu screen and never to the persistent scene. 
    }

    private void UpdateTimeRemainingDisplay() 
    {
        timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
        if (timeRemainingDisplayText.text == null)
        {
            Debug.Log(timeRemainingDisplayText);
        }
        else
        {
            Debug.Log("No time remain");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f) //when out of time
            {
                EndRound(); //call it a ground 
            }

        }
    }
}
