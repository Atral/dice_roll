using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public Transform Dice;


    public void Button_Click()
    {   
        Debug.Log("Button Clicked");
        bool spawned = false;
        Vector3[] positionArray = new [] {
            new Vector3(-3.0f, 2.0f, 0.0f), new Vector3(-3.0f, 2.0f, -2.0f),
            new Vector3(-3.0f, 2.0f, 2.0f), new Vector3(-3.0f, 4.0f, 0.0f),
            new Vector3(-3.0f, 4.0f, -2.0f), new Vector3(-3.0f, 4.0f, 2.0f),
            new Vector3(-3.0f, 2.0f, -4.0f), new Vector3(-3.0f, 2.0f, 4.0f),
            new Vector3(-3.0f, 4.0f, -4.0f), new Vector3(-3.0f, 4.0f, 4.0f)
            };
        
        var radius = 0.7f;
        int i = 0;

        while(i < positionArray.Length && !spawned){
            if (!(Physics.CheckSphere (positionArray[i], radius))) {
                Debug.Log("Spawn point found");
                var go = Instantiate(Dice, positionArray[i], transform.rotation);
                spawned = true;
            } else {
                i++;
            }
        }

        
    }
}
