/**** 
 * Created by: Akram Taghavi-Burrs
 * Date Created: Feb 23, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: Feb 28, 2022
 * 
 * Description: Updates HUD canvas referecing game manager
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour
{
    /*** VARIABLES ***/

    GameManager gm; //reference to game manager

    [Header("Canvas SETTINGS")]
    public Text jobTitleTextbox; //textbox for Job Title
    public Text bestTitleTextbox; //textbox for the best job title
    public Text itemsBaggedTextbox; //textbox for the items baggged
    public Text highScoreTextbox; //textbox for highscore
    public Text timerTextbox; //textbox for the timer 
    public Timer gameTimer;

    

    private void Start()
    {
        gm = GameManager.GM; //find the game manager

        SetHUD();
    }//end Start

    // Update is called once per frame
    void Update()
    {
        GetGameStats();
        SetHUD();
    }//end Update()

    void GetGameStats()
    {
        
    }

    void SetHUD()
    {
        //if texbox exsists update value
        if (timerTextbox) { SetTimer(); }
        if (jobTitleTextbox) { SetTitle(); }

    }//end SetHUD()

    void SetTimer()
    {
        timerTextbox.text = "0:" + gameTimer.GetTimeToDisplay().ToString("00");
    }

    void SetTitle()
    {
        jobTitleTextbox.text = "Job Title: " + gm.GetTitle();
    }

}
