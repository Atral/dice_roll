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
    }
    void RollDice()
    {
        if(!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500),Random.Range(0,500), Random.Range(0,500));

        }
    }
}
