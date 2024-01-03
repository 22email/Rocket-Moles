using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; 
    public TextMeshProUGUI gameOverScoreText; 
    private float score; // moles clicked

    void Start()
    {
        score = 0;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.SetText("<sprite=0> " + score.ToString());
        gameOverScoreText.SetText("You Whacked " + score.ToString() + " Moles!");
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.SetText("<sprite=0> " + score.ToString());
    }
}
