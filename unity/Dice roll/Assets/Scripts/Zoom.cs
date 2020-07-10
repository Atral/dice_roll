using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{   
    Vector3 groundOffset;
    Vector3 target;
    Vector3 camSmoothDamp;
    Vector3 camAngle = new Vector3(90, 90, 0);
    GameObject[] dices;
    string TagField;
    // Start is called before the first frame update
    
    public void Update()
    {   
        TagField = "d20";
        dices = GameObject.FindGameObjectsWithTag(TagField);
    }

    public void CenterCamera(){

        if(dices.Length == 1){
            groundOffset = new Vector3(0.0f, 3.0f, 0.0f);
            camera.transform.position = dices[0].transform.position - groundOffset;
            camera.transform.angle = camAngle;
        }
    }
}
