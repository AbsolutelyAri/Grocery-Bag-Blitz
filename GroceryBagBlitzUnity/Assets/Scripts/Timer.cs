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
    private bool timeStarted;
    public float timeRemaining;
    public float maxTime = 45f;

    private bool test;

    [SerializeField]
    GameManager gm;

    private void Awake()
    {
        timeRemaining = maxTime;
        
    }

    private void Start()
    {
        gm = GameManager.GM;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gm.playing) { return; }

        if (test != timeStarted) { Debug.Log("Something changed " + timeStarted); }

        if (timeStarted == true)
        {
            
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timeStarted = false;
                gm.endMsg = "Too Slow!";
                gm.GameOver();
            }
        }
        test = timeStarted;
    }

    // Starts the timer
    public void TimerStart()
    {
        Debug.Log("Timer going");
        timeStarted = true;
        timeRemaining = maxTime;
    }

    // Stops the timer
    public void TimerStop()
    {
        timeStarted = false;
        Debug.Log("Timer Stopped " + timeStarted);
    }

    public int GetTimeToDisplay()
    {
        return (int)Math.Ceiling(timeRemaining);
    }
}
