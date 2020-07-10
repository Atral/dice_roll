using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zoom : MonoBehaviour
{   
    Vector3 groundOffset;
    Vector3 target = new Vector3(2.0f, 1.39f, 2.0f);
    Vector3 camSmoothDamp;
    Vector3 initCamPos;
    Quaternion initRot;
    GameObject[] dices;
    string TagField;
    bool stationary = false;

    List<Vector3> positionArray = new List<Vector3>(){
        new Vector3(0.0f, 1.39f, -2.0f), new Vector3(0.0f, 1.39f, 2.0f), new Vector3(0.0f, 1.39f, 2.0f),
        new Vector3(2.0f, 1.39f, -2.0f), new Vector3(2.0f, 1.39f, 2.0f), new Vector3(2.0f, 1.39f, 2.0f),
        new Vector3(4.0f, 1.39f, -2.0f), new Vector3(4.0f, 1.39f, 2.0f), new Vector3(4.0f, 1.39f, 2.0f),
    };
    // Start is called before the first frame update

    private static bool Stationary(GameObject tDice){
        return tDice.GetComponent<Rigidbody>().IsSleeping();
    }
    
    public void Start(){
        initCamPos = transform.position;
        initRot = transform.rotation;
    }
    
    public void Update()
    {   
        TagField = "d20";
        dices = GameObject.FindGameObjectsWithTag(TagField);

        if(Array.TrueForAll(dices, Stationary)){
            stationary = true;
        }
    }

    public void CenterCamera(){

        List<Vector3> tempPos;
        tempPos = positionArray;
        Debug.Log(tempPos);

        if(stationary){

            var euler = transform.eulerAngles;
            euler.x = 90.0f;
            euler.y = 90.0f;
            euler.z = 0.0f;

            if(dices.Length == 1){
                groundOffset = new Vector3(0.0f, -3.0f, 0.0f);

                transform.position = dices[0].transform.position - groundOffset;
                transform.eulerAngles = euler;

            }else if(dices.Length > 0){
                groundOffset = new Vector3(0.0f, (dices.Length * -3.0f), 0.0f);

                foreach(GameObject dice in dices){
                    dice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                    dice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    

                    dice.transform.position = tempPos[0];
                    tempPos.RemoveAt(0);        
                }
                
                transform.position = target - groundOffset;
                transform.eulerAngles = euler;
            }
        }
    }

    public void ResetCamera(){
        transform.position = initCamPos;
        transform.rotation = initRot;
    }
}
