using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float speed = 1.0f;
    bool active = false;
    string tagField = "d20";

    List<Vector3> positionArray = new List<Vector3>(){
        new Vector3(0.0f, 1.39f, 0f), new Vector3(0.0f, 1.39f, -2.0f),  new Vector3(0.0f, 1.39f, 2.0f),
        new Vector3(2.0f, 1.39f, 0f), new Vector3(2.0f, 1.39f, -2.0f), new Vector3(2.0f, 1.39f, 2.0f),
        new Vector3(4.0f, 1.39f, 0f), new Vector3(4.0f, 1.39f, -2.0f), new Vector3(4.0f, 1.39f, 2.0f),
    };

    GameObject[] dices;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        dices = GameObject.FindGameObjectsWithTag(tagField);

        foreach(GameObject dice in dices)
        {
            if(active){

                for(int i = 0; i < dices.Length; i++){
                    Rigidbody rb = dices[i].GetComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = false;
                    rb.detectCollisions = false;

                    Vector3 target = positionArray[i];
                    float step =  3* speed * Time.deltaTime; // calculate distance to move
                    if(dices[i].transform.position != target){
                        dices[i].transform.position = Vector3.MoveTowards(dices[i].transform.position, target, step);
                    }
                    

                }
                
            }
        }
    }

    public void PositionDice(){
        active = true;
    }

    public void StopMoving(){
        active = false;
    }
    
}
