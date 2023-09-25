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

    public void PlayAgainButton()
    {

        var existingBullets = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject bullet in existingBullets) 
        {
            Destroy(bullet);
        }

        playButton.interactable = false;
        Time.timeScale = 1f;
        canvasGroup.alpha = 0f;
        onPlayAgain.Invoke();
    }

}
