using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI scoreText; 
    [HideInInspector] public float score; // moles clicked

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore()
    {
        score++;
        scoreText.SetText("<sprite=0> " + score.ToString());
    }

    public void resetScore()
    {
        score = 0;
        scoreText.SetText("<sprite=0> " + score.ToString());
    }
}
