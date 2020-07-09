using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    List<Vector3> startPos;
    public GameObject[] dices;
    public GameObject dice;
    public string TagField;
    // Start is called before the first frame update
    void Start()
    {
        dices = GameObject.FindGameObjectsWithTag(TagField);

        foreach(GameObject dice in dices){
            Debug.Log(dice.transform.position);
            startPos.Add(dice.transform.position);
        }
    }

    public void ResetPos(){
            Debug.Log("Resetting position");
            Debug.Log(startPos);
            for(int i = 0; i < dices.Length; i++){
                dice.transform.position = startPos[i];
            }
        }
}
