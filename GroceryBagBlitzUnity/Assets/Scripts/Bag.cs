/**** 
 * Created by: Krieger
 * Date Created: March 3, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: March 3, 2022
 * 
 * Description: Handles the behavior of the bag.
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Timer timer; //reference to the level timer
    public float speed = 5f; //speed that the bag moves at
    public float sendXPosition = -25; //X position the bag will turn around at

    [Header("Set Dynamically")]
    public Vector3 startPosition;

    private bool sent;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GM;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (sent) //if the bag is moving away
        {
            if(transform.position.x > sendXPosition)
            {
                Vector3 posVec = transform.position;
                posVec.x -= speed * Time.deltaTime;
                transform.position = posVec;
            }
            else
            {
                sent = false;
                gm.CheckItems(); //check if all the items crossed the deletion line
            }
        }
        else if(startPosition.x > transform.position.x) //if the bag isn't moving away and hasn't returned to the starting position
        {
            Vector3 posVec = transform.position;
            posVec.x += speed * Time.deltaTime;
            transform.position = posVec;
        }
    }

    // Sends the bag off screen, then back a moment later.
    public void SendOff()
    {
        timer.TimerStop(); //stop the timer
        sent = true;
    }
}
