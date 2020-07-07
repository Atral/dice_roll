using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGlow : MonoBehaviour
{
    [SerializeField] GameObject[] sidesF;
    DiceStat diceStat;
    // Start is called before the first frame update
    void Start()
    {
        diceStat = gameObject.GetComponent<DiceStat>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void HighlightSides()
    {
        for(int i = 0; i < sidesF.Length; i++)
        {
            sidesF[i].SetActive(false);
        }
        sidesF[diceStat.side - 1].SetActive(true);
    }
}
