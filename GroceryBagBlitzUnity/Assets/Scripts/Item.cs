/**** 
 * Created by: Krieger
 * Date Created: Feb 28, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: Feb 28, 2022
 * 
 * Description: Script that determines of the behavior of basic grocery items. Makes items follow the mouse when lmb is held down
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private bool isClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked)
        {
            //follow mouse until mouse clicked again
        } 
        else if (Input.GetMouseButton(0))
        {
            //check if this is the object being clicked, if so enable isClicked
        }
    }
}
