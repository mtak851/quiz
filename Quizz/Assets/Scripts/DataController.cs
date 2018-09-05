using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour  //Persists through the scenes!
    //then you can load scenes, 
    //this is attached to the gameobject, so mono behaviour
    //later on we can put multiple attempt 

{
    public RoundData[] allRoundData; //array of object to expand later on 


    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject); //when we load the new scene it persistes!

        SceneManager.LoadScene("MenuScreen");
    }

    public RoundData GetCurrentRoundData()  //th purpose of this script is to supply the round data to the game controller when we get to that scene!
    {
        return allRoundData[0]; //the number of the rounds, round 0 for now but later on we can put what data at how many rounds to return.
    }

    // Update is called once per frame
    void Update()
    {

    }
}