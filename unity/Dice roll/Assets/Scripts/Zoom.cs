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

    List<Vector3> positionArray = new List<Vector3>()
    {
        new Vector3(0.0f, 1.39f, -2.0f), new Vector3(0.0f, 1.39f, 0f), new Vector3(0.0f, 1.39f, 2.0f),
        new Vector3(2.0f, 1.39f, -2.0f), new Vector3(2.0f, 1.39f, 0f), new Vector3(2.0f, 1.39f, 2.0f),
        new Vector3(4.0f, 1.39f, -2.0f), new Vector3(4.0f, 1.39f, 0f), new Vector3(4.0f, 1.39f, 2.0f),
    };

    // Start is called before the first frame update
    public void Start()
    {
        // Stores camera starting position
        initCamPos = transform.position;
        initRot = transform.rotation;
    }
    
    public void Update()
    {   
        // Finds all the dice
        TagField = "d20";
        dices = GameObject.FindGameObjectsWithTag(TagField);
    }

    // Centers the camera on the dice
    public void CenterCamera()
    {

        List<Vector3> tempPos;
        tempPos = positionArray;
        Debug.Log(tempPos);

        var euler = transform.eulerAngles;
        euler.x = 90.0f;
        euler.y = 90.0f;
        euler.z = 0.0f;

        if(dices.Length > 0)
        {
            for(int i = 0; i < dices.Length; i++){

                dices[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                dices[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                dices[i].transform.position = positionArray[i];
            }
        }

        //Calcuates the correct camrea position depending on the number of dice
        Vector3 totalPosition = new Vector3(-0.02f,0,0);
        
        foreach(GameObject dice in dices)
        {
            totalPosition += dice.transform.position;
        }
        Vector3 center = totalPosition / dices.Length;

        float yOff;

        if(dices.Length == 1)
        {
            yOff = -3.0f; 
        }
        else if(dices.Length == 2)
        {
            yOff = -4.0f;
        }
        else
        {
            yOff = -6.0f;
        }

        groundOffset = new Vector3(0.0f, yOff, 0.0f);

        //Changes camera position and rotation
        transform.position = center - groundOffset;
        transform.eulerAngles = euler;
    }

    public void ResetCamera()
    {
        transform.position = initCamPos;
        transform.rotation = initRot;
    }
}