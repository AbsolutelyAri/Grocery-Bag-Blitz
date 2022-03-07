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
    public float velocity = 5f; //speed that the bag moves at
    public float sendXPosition = -25; //X position the bag will turn around at

    [Header("Set Dynamically")]
    public Vector3 startPosition;

    private bool sent;
    private bool stopped;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GM;
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (sent) //if the bag is moving away
        {
            if(transform.position.x < sendXPosition)
            {
                sent = false;
                SendBack();
                gm.CheckItems(); //check if all the items crossed the deletion line
            }
        }
        else if(startPosition.x < transform.position.x && !stopped) //if the bag isn't moving away and hasn't returned to the starting position
        {
            StopBag();
        }
    }

    // Sends the bag off screen, then back a moment later.
    public void SendOff()
    {
        Debug.Log("Sent");
        timer.TimerStop(); //stop the timer
        sent = true;
        stopped = false;
        Vector3 velocityVector = GetComponent<Rigidbody>().velocity;
        velocityVector.x = -velocity;
        Debug.Log(velocityVector);
        GetComponent<Rigidbody>().velocity = velocityVector;
    }

    private void SendBack()
    {
        Debug.Log("Returning");
        Vector3 velocityVector = GetComponent<Rigidbody>().velocity;
        velocityVector.x = velocity;
        GetComponent<Rigidbody>().velocity = velocityVector;
    }

    private void StopBag()
    {
        Debug.Log("Stopping");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        stopped = true;
        
    }
}
