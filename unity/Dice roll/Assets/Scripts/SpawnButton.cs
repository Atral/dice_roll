using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{   
    public Transform Dice;

    List<Vector3> positionArray = new List<Vector3>(){
            new Vector3(-3.0f, 2.0f, 0.0f), new Vector3(-3.0f, 2.0f, -2.0f),
            new Vector3(-3.0f, 2.0f, 2.0f), new Vector3(-3.0f, 4.0f, 0.0f),
            new Vector3(-3.0f, 4.0f, -2.0f), new Vector3(-3.0f, 4.0f, 2.0f),
            new Vector3(-3.0f, 2.0f, -4.0f), new Vector3(-3.0f, 2.0f, 4.0f),
            new Vector3(-3.0f, 4.0f, -4.0f), new Vector3(-3.0f, 4.0f, 4.0f)
    }
    ;
    float radius = 0.6f;

    public void Button_Click()
    {   
        Debug.Log("Spawning dice");
        bool spawned = false;
        
        int i = 0;
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

    public void Reset(){
        Debug.Log("Resetting");
        GameObject[] dices = GameObject.FindGameObjectsWithTag("d20");
        List<Vector3> posCopy = positionArray;

        foreach(GameObject d in dices){
            if(!(posCopy.Contains(d.transform.position))){
                int i = 0;
                
                while(i < posCopy.Count){
                    if (!(Physics.CheckSphere (positionArray[i], radius))) {
                        Debug.Log("Reset point found");
                        d.transform.position = positionArray[i];
                        d.rigidbody.useGravity = true;
                        posCopy.RemoveAt(i);
                        i = 100;
                    }else{
                        i++;
                    }
                }
            }   
        }   
    }
}
