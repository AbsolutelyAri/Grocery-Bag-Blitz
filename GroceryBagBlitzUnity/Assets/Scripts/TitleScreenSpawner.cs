
/**** 
 * Created by: Krieger
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: Krieger
 * Last Edited: March 7, 2022
 * 
 * Description: Script that spawns a rain of random items on the Title Screen
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenSpawner : MonoBehaviour
{
    public float itemSpawnChance = .2f;
    public float itemSpawnRangeX = 10;
   
    private GameManager gm;
    private List<GameObject> items;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GM;
        items = gm.items;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float itemSpawn = Random.Range(0f, 1f);
        if(itemSpawn < itemSpawnChance)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        int itemSelected = Random.Range(0, items.Count);
        GameObject newItem = Instantiate<GameObject>(items[itemSelected]);
        Vector3 itemPos = this.transform.position;
        float newX = Random.Range(-itemSpawnRangeX, itemSpawnRangeX);
        itemPos.x = newX;
        newItem.transform.position = itemPos;
    }
}
