using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceStat : MonoBehaviour
{
    [SerializeField] Transform[] diceSides;
    public int side = 0;
    Rigidbody rb;
    
    bool hasLanded;
    bool thrown;

    Vector3 initPos;

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
            CheckDiceSide();
        
        }
        else if(rb.IsSleeping() && hasLanded && side == 0){
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

    private int MaxValue(double[] arr){
        double max = 0.0;
        int location = 0;

        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] > max)
            {
                max = arr[i];
                location = i;
            }
        }
        return location;
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

    void CheckDiceSide()
    {   
        double[] yPositions = new double[diceSides.Length];

        for(int i = 0; i < diceSides.Length; i++)
        {
            yPositions[i] = diceSides[i].position.y;
        }

        side = MaxValue(yPositions);
        Debug.Log("Dice landed on " + (side + 1));
    }
    
}