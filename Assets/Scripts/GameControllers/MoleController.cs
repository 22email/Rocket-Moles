using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController: MonoBehaviour
{
    public Mole[] molesArray;

    private int lastMole;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Mole mole in molesArray)
        {
            mole.gameObject.SetActive(false);
        }

        GenRandMole();
    }

    void Awake()
    {
        molesArray = FindObjectsOfType<Mole>(); 

        lastMole = 1;
    }

    public void GenRandMole()
    {
        int moleNum = 0;
        
        do 
        {
            moleNum = Random.Range(0, molesArray.Length);
        } while(moleNum == lastMole);

        molesArray[moleNum].gameObject.SetActive(true);

        lastMole = moleNum;
    }

    public void CleanBoard()
    {
        molesArray[lastMole].gameObject.SetActive(false);
        GenRandMole();
    } 

}
