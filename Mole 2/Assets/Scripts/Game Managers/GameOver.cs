using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverMenu;
    public CanvasGroup canvasGroup;
    public Button playButton;
    public UnityEvent onPlayAgain;
    public void playAgainButton()
    {
        playButton.interactable = false;
        Time.timeScale = 1f;
        canvasGroup.alpha = 0f;
        onPlayAgain.Invoke();
    }

}
