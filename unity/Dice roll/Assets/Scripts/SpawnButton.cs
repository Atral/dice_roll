using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{   
    public Transform Dice;
    public GameObject[] dices;

    public List<Vector3> positionArray = new List<Vector3>(){
        new Vector3(-3.0f, 2.0f, 0.0f), new Vector3(-3.0f, 2.0f, -2.0f),
        new Vector3(-3.0f, 2.0f, 2.0f), new Vector3(-3.0f, 4.0f, 0.0f),
        new Vector3(-3.0f, 4.0f, -2.0f), new Vector3(-3.0f, 4.0f, 2.0f),
        new Vector3(-3.0f, 2.0f, -4.0f), new Vector3(-3.0f, 2.0f, 4.0f),
        new Vector3(-3.0f, 4.0f, -4.0f), new Vector3(-3.0f, 4.0f, 4.0f)
    };

    float radius = 0.6f;

    private void spawn(){
        int i = 0;
        bool spawned = false;

        while(i < positionArray.Count && !spawned){

            if (!(Physics.CheckSphere (positionArray[i], radius))) {
                
                Debug.Log("Spawn point found");
                var go = Instantiate(Dice, positionArray[i], transform.rotation);
                spawned = true;

            } else {
                i++;
            }
        }
    }

    public void Button_Click()
    {   
        Debug.Log("Spawning dice");
        dices = GameObject.FindGameObjectsWithTag("d20");
        
        if(dices.Length < 10){
            spawn();
        }else{
            Debug.Log("Dice limit reached");
        }
    }
}
