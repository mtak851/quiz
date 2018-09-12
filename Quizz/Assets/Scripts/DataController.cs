using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour  //Persists through the scenes!
                                             //then you can load scenes, 
                                             //this is attached to the gameobject, so mono behaviour
                                             //later on we can put multiple attempt 

{
    private RoundData[] allRoundData; //array of object to expand later on  //now load fronm the json file, so we call it from start

    private PlayerProgress playerProgress;
    private string gameDataFileName = "data.Json";



    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject); //when we load the new scene it persistes!
        LoadGameData ();
        LoadPlayerProgress ();

        SceneManager.LoadScene("MenuScreen");

    }

    public RoundData GetCurrentRoundData()  //th purpose of this script is to supply the round data to the game controller when we get to that scene!
    {
        return allRoundData[0]; //the number of the rounds, round 0 for now but later on we can put what data at how many rounds to return.
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }



    private void LoadPlayerProgress()    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object

    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }


    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }


    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);



        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData, game controller also has the round data 
            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

}






