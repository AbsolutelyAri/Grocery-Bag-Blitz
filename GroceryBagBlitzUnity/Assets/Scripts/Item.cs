/**** 
 * Created by: Krieger
 * Date Created: Feb 28, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: March 3, 2022
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

    GameManager gm;

    private bool isClicked = false;
    private Vector3 lastMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        lastMousePosition = transform.position;
        gm = GameManager.GM;
    }

    // Update is called once per frame
    void Update()
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
                Debug.Log(velocityVector + " normalized");
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
        Debug.Log("object clicked " + transform.position);
        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        isClicked = false;
        GetComponent<Rigidbody>().useGravity = true;
        Debug.Log("object unclicked");
        Cursor.visible = true;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if(other.gameObject.tag == "DeletionZone")
        {
            Debug.Log("collided with deletion zone");
            //tell GameManager this object was deleted
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "GameOverZone")
        {
            //tell GameManager this object was dropped and call GameOver
            gm.GameOver();
            Destroy(this.gameObject);
        }
    }
}
