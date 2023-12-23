using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI scoreText; 
    public TextMeshProUGUI gameOverScoreText; 
    private float score; // moles clicked

    void Start()
    {
        score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore()
    {
        score++;
        scoreText.SetText("<sprite=0> " + score.ToString());
        gameOverScoreText.SetText("You Whacked " + score.ToString() + " Moles!");
    }

    public void resetScore()
    {
        score = 0;
        scoreText.SetText("<sprite=0> " + score.ToString());
    }
}
