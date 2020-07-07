using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    
    bool hasLanded;
    bool thrown;

    Vector3 initPos;

    public int diceValue;

    public DiceSide[] diceSides;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;
        rb.useGravity = false;
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            RollDice();
        }
        //Checks dice side value if the dice is stationary and has been thrown
        if(rb.IsSleeping() && !hasLanded && thrown){
            hasLanded = true;
            rb.useGravity = false;
            SideValueCheck();
        
        }
        else if(rb.IsSleeping() && hasLanded && diceValue == 0){
            Debug.Log("Dice did not land on a side.");
            RollAgain();
        }
    }
    void RollDice(){
        if(!thrown && !hasLanded){
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500),Random.Range(0,500), Random.Range(0,500));
        }

        else if(thrown && hasLanded){
            Reset();
        }
    }
    
    void Reset(){
        transform.position = initPos;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    void RollAgain(){
        Debug.Log("Re-rolling.");
        Reset();
        thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500),Random.Range(0,500), Random.Range(0,500));
    }

    void SideValueCheck(){
        diceValue = 0;
        foreach (DiceSide side in diceSides){

            if(side.OnGround()){
                diceValue = side.sideValue;
                Debug.Log(diceValue + " has been rolled.");
            }
        } 

    }
}
