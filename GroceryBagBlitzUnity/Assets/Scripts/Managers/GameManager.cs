/**** 
 * Created by: Akram Taghavi-Burrs
 * Date Created: Feb 23, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: Feb 28, 2022
 * 
 * Description: Basic GameManager Template for Grocery Bag Blitz
****/

/** Import Libraries **/
using System; //C# library for system properties
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //libraries for accessing scenes


public class GameManager : MonoBehaviour
{
    /*** VARIABLES ***/

    #region GameManagerSingleton
    static private GameManager gm; //refence GameManager
    static public GameManager GM { get { return gm; } } //public access to read only gm 

    //Check to make sure only one gm of the GameManager is in the scene
    void CheckGameManagerIsInScene()
    {

        //Check if instnace is null
        if (gm == null)
        {
            gm = this; //set gm to this gm of the game object
            Debug.Log(gm);
        }
        else //else if gm is not null a Game Manager must already exsist
        {
            Destroy(this.gameObject); //In this case you need to delete this gm
        }
        DontDestroyOnLoad(this); //Do not delete the GameManager when scenes load
        Debug.Log(gm);
    }//end CheckGameManagerIsInScene()
    #endregion

    #region Variables
    [Header("GENERAL SETTINGS")]
    public string gameTitle = "Grocery Bag Blitz";  //name of the game
    public string gameCredits = "Made by Krieger (the great and powerful)"; //game creator(s)
    public string copyrightDate = "Copyright " + thisDay; //date created

    [Header("GAME SETTINGS")]

    [SerializeField] //Access to private variables in editor
    private int defaultHighScore = 0;
    static public int highScore = 0; // the default High Score
    public int HighScore { get { return highScore; } set { highScore = value; } }//access to private variable highScore [get/set methods]

    [Space(10)]

    static public int score = 0;  //score value
    public int Score { get { return score; } set { score = value; } }//access to private variable died [get/set methods]
    public int wavesSurvived; //number of waves the player has survived, used for job titles

    [SerializeField] //Access to private variables in editor
    [Tooltip("Check to test player lost the level")]
    private bool levelLost = false;//we have lost the level (ie. player died)
    public bool LevelLost { get { return levelLost; } set { levelLost = value; } } //access to private variable lostLevel [get/set methods]

    [Space(10)]
    public string defaultEndMessage = "You Were Fired";//the end screen message, depends on winning outcome
    [HideInInspector] public string endMsg; //the end screen message, depends on winning outcome

    [Header("SCENE SETTINGS")]
    [Tooltip("Name of the start scene")]
    public string startScene;

    [Tooltip("Name of the game over scene")]
    public string gameOverScene;

    [Tooltip("Count and name of each Game Level (scene)")]
    public string gameLevel; //names of level
    [HideInInspector]
    private int loadLevel; //what level from the array to load

    public static string currentSceneName; //the current scene name

    //Game State Varaiables
    [HideInInspector] public enum gameStates { Idle, Playing, Death, GameOver, BeatLevel };//enum of game states
    [HideInInspector] public gameStates gameState = gameStates.Idle;//current game state
    public bool playing = false;

    //Timer Varaibles
    private float currentTime; //sets current time for timer


    //reference to system time
    private static string thisDay = System.DateTime.Now.ToString("yyyy"); //today's date as string

    //how many items currently exist
    public int numberOfItems = 0;

    //variables for spawning items
    [Space(5)]
    [Header("ITEM SPAWNING")]
    public List<GameObject> items = new List<GameObject>(); //list of item prefabs that will be spawned
    int itemsToSpawn = 5;
    public Vector3 itemSpawnPosition = new Vector3(-5f, 5f, 0f);
    public float furthestBackX = 30;
    public float conveyorSpeed = 4;
    [HideInInspector]
    public float totalSpace = 0;

    [Space(5)]
    [Header("JOB TITLES")]
    //list of job titles
    public List<string> titles = new List<string>();
    public string currentTitle; //current title as a string
    private static int titleIndex = 0; //index of the player's current title
    public int bestTitleIndex = 0;

    private int nextTitleRequirement = 1;

    [Space(5)]
    [Header("MISC SETTINGS")]
    public float timerMultiplier = .8f; //multiplier that decreases maxTime

    #endregion


    /*** MEHTODS ***/

    //Awake is called when the game loads (before Start).  Awake only once during the lifetime of the script instance.
    void Awake()
    {
        //runs the method to check for the GameManager
        CheckGameManagerIsInScene();

        //store the current scene
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        //Get the saved high score
        GetHighScore();
        currentTitle = titles[titleIndex];

        endMsg = defaultEndMessage;
    }//end Awake()


    // Update is called once per frame
    private void Update()
    {
        //if ESC is pressed , exit game
        if (Input.GetKey("escape")) { ExitGame(); }

        /*
        if (gameState == gameStates.Playing)
        {
            //if we have died and have no more lives, go to game over
            if (levelLost) { GameOver(); }
            

        }//end if (gameState == gameStates.Playing)
        */
        //Check Score
        CheckScore();



    }//end Update


    //LOAD THE GAME FOR THE FIRST TIME OR RESTART
    public void StartGame()
    {
        //SET ALL GAME LEVEL VARIABLES FOR START OF GAME
        numberOfItems = 0;
        SceneManager.LoadScene(gameLevel); //load first game level

        gameState = gameStates.Playing; //set the game state to playing
        playing = true;

        score = 0; //set starting score
        titleIndex = 0; //set starting title
        currentTitle = titles[0];
        CheckScore();
        CheckTitles();

        if (highScore <= defaultHighScore)
        {
             highScore = defaultHighScore; //set the high score to defulat
             PlayerPrefs.SetInt("MostBagged", highScore); //update high score PlayerPref
        }//end if (highScore <= defaultHighScore)
        

        endMsg = defaultEndMessage; //set the end message default

        GetComponent<Timer>().maxTime = 30;
        GetComponent<Timer>().TimerStart();
        nextTitleRequirement = 1;

    }//end StartGame()



    //EXIT THE GAME
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exited Game");
    }//end ExitGame()


    //GO TO THE GAME OVER SCENE
    public void GameOver()
    {
        playing = false;
        Cursor.visible = true;
        gameState = gameStates.GameOver; //set the game state to gameOver

        SceneManager.LoadScene(gameOverScene); //load the game over scene
        Debug.Log("Gameover");
    }

    void CheckScore()
    { //This method manages the score on update. Right now it just checks if we are greater than the high score.
        //if the score is more than the high score
        if (score > highScore)
        {
            highScore = score; //set the high score to the current score
            PlayerPrefs.SetInt("MostBagged", highScore); //set the playerPref for the high score
        }//end if(score > highScore)
        if(titleIndex > bestTitleIndex)
        {
            bestTitleIndex = titleIndex;
        }

    }//end CheckScore()

    void GetHighScore()
    {//Get the saved highscore

        //if the PlayerPref alredy exists for the high score
        if (PlayerPrefs.HasKey("MostBagged"))
        {
            Debug.Log("Has Key");
            highScore = PlayerPrefs.GetInt("MostBagged"); //set the high score to the saved high score
        }//end if (PlayerPrefs.HasKey("HighScore"))
        if (PlayerPrefs.HasKey("BestTitleIndex"))
        {
            Debug.Log("Has BestTitleIndex");
            bestTitleIndex = PlayerPrefs.GetInt("BestTitleIndex");
        }

        PlayerPrefs.SetInt("MostBagged", highScore); //set the playerPref for the high score
        PlayerPrefs.SetInt("BestTitleIndex", bestTitleIndex);
    }//end GetHighScore()

    //check if the player has earned a new title
    void CheckTitles()
    {
        if (wavesSurvived >= nextTitleRequirement)
        {
            Debug.Log("title upgrade earned " + titles.Count);
            if (titles.Count > (titleIndex + 1))
            {
                titleIndex++;
                currentTitle = titles[titleIndex];
                Debug.Log(currentTitle);
            }

            nextTitleRequirement += 1;
        }
    }

    //spawn a wave of items
    public void SpawnItems()
    {
        totalSpace = 0;
        bool secondLevel = false;

        for(int i = 0; i < itemsToSpawn; i++)
        {
            int newItemIndex = UnityEngine.Random.Range(0, items.Count);
            GameObject newItem = Instantiate<GameObject>(items[newItemIndex]);
            Vector3 itemPos = itemSpawnPosition;
            itemPos.x += totalSpace;
            if(totalSpace > furthestBackX)
            {
                totalSpace = 0;
                itemSpawnPosition.y += 5;
                secondLevel = true;
            }
            newItem.transform.position = itemPos;
            numberOfItems++;
        }
        if (secondLevel)
        {
            itemSpawnPosition.y -= 5;
        }
        GetComponent<Timer>().maxTime *= timerMultiplier;
    }

    //reduce the number of items by 1
    public void DecrimentItems()
    {
        numberOfItems--;
        score++;
    }

    //check if there are any items remaining
    public void CheckItems()
    {
        Debug.Log("CheckItems called");
        if (numberOfItems > 0)
        {
            endMsg = "You missed " + numberOfItems;
            GameOver();

        }
        else
        {
            wavesSurvived++;
            CheckTitles();
            SpawnItems();
        }
        GetComponent<Timer>().TimerStart();
    }

    public string GetTitle()
    {
        return currentTitle;
    }

    public string GetBestTitle()
    {
        return titles[bestTitleIndex];
    }
}
