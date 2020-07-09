using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveButton : MonoBehaviour
{   
    public GameObject dice;
    public GameObject[] dices;
    public string TagField;

    public void Remove(){
            dices = GameObject.FindGameObjectsWithTag(TagField);
            Destroy(dices[dices.Length-1]);
        }
        
}
