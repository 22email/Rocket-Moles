using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button playButton;
    public UnityEvent onPlayAgain;
    public void playAgainButton()
    {
        playButton.interactable = false;
        Time.timeScale = 1f;
        canvasGroup.alpha = 0f;
        onPlayAgain.Invoke();
    }

}
