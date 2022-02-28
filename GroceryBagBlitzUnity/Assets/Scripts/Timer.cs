/**** 
 * Created by: Krieger
 * Date Created: Feb 28, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: Feb 28, 2022
 * 
 * Description: Script that manages the timer for the game.
****/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool timeStarted = true;
    public float timeRemaining;
    public float maxTime = 45f;

    GameManager gm;

    private void Awake()
    {
        timeRemaining = maxTime;
        gm = GameManager.GM;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStarted)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0)
            {
                timeRemaining = 0;
                timeStarted = false;
                gm.LevelLost = true;
            }
        }
    }

    // Starts the timer
    public void TimerStart()
    {
        timeStarted = true;
        timeRemaining = maxTime;
    }

    // Stops the timer
    public void TimerStop()
    {
        timeStarted = false;
    }

    public int GetTimeToDisplay()
    {
        return (int)Math.Ceiling(timeRemaining);
    }
}
