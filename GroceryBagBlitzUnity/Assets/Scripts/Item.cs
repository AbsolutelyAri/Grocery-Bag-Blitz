/**** 
 * Created by: Krieger
 * Date Created: Feb 28, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: March 7, 2022
 * 
 * Description: Script that determines of the behavior of basic grocery items. Makes items follow the mouse when lmb is held down
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float easingPercentage = 0.3f; //easing on the velocity vector
    public float maxMagnitude = 5f; //fastest the item is allowed to move when following the mouse
    public float width = 4;

    GameManager gm;

    private bool isClicked = false;
    private Vector3 lastMousePosition;
    // Start is called before the first frame update
    void Awake()
    {
        lastMousePosition = transform.position;
        gm = GameManager.GM;
        gm.totalSpace += width;
    }

    
    void FixedUpdate()
    {
        if (isClicked)
        {
            Vector3 mousePosition2D = Input.mousePosition;

            Vector3 mousePosition3D = Camera.main.ScreenToWorldPoint(mousePosition2D);
            mousePosition3D.z = transform.position.z;
            //transform.position = mousePosition3D; 

            //make the object follow the mouse without going through solid objects
            Vector3 velocityVector = (mousePosition3D - lastMousePosition) / Time.deltaTime;

            if(velocityVector.magnitude > maxMagnitude) 
            {
                velocityVector.Normalize();
                velocityVector *= maxMagnitude;
            }

            GetComponent<Rigidbody>().velocity = Vector3.Lerp(velocityVector, GetComponent<Rigidbody>().velocity, easingPercentage);




            lastMousePosition = mousePosition3D;
        }
    }

    private void OnMouseDown()
    {
        isClicked = true;
        GetComponent<Rigidbody>().isKinematic = false;
        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        isClicked = false;
        GetComponent<Rigidbody>().useGravity = true;
        Cursor.visible = true;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "DeletionZone")
        {
            //tell GameManager this object was deleted
            gm.DecrimentItems();
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "GameOverZone")
        {
            if (gm.playing)
            {
                //tell GameManager this object was dropped and call GameOver
                gm.endMsg = "You dropped something";
                gm.GameOver();
            }
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Conveyor" && !isClicked)
        {
            Vector3 pos = transform.position;
            pos.x -= gm.conveyorSpeed * Time.deltaTime;
            transform.position = pos;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Conveyor" && !isClicked)
        {
            Vector3 pos = transform.position;
            pos.x -= gm.conveyorSpeed * Time.deltaTime;
            transform.position = pos;
            
        }
    }




}
