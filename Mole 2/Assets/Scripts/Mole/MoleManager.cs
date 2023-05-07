using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager : MonoBehaviour
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

        genRandMole();
    }

    void Awake()
    {
        molesArray = FindObjectsOfType<Mole>(); 

        lastMole = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void genRandMole()
    {
        int moleNum = 0;
        
        do 
        {
            moleNum = Random.Range(0, molesArray.Length);
        }while(moleNum == lastMole);

        molesArray[moleNum].gameObject.SetActive(true);

        lastMole = moleNum;
    }

    public void cleanBoard()
    {
        molesArray[lastMole].gameObject.SetActive(false);
        genRandMole();
    } 

}
