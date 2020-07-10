﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceTotal : MonoBehaviour
{
    int diceTotal;
    Text totalDisplay;

    public void Start(){
        diceTotal = 0;
        totalDisplay = GameObject.FindGameObjectWithTag("DiceTotal").GetComponent<Text>();

    }

    public void setTotal(int diceValue)
    {
        diceTotal += diceValue;
        totalDisplay.text = "Total: " + diceTotal.ToString();
    }

    public void resetTotal(){
        diceTotal = 0;
    }

}
