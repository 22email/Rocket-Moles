using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameLoopController : MonoBehaviour
{
    [SerializeField]
    private float gameTime;

    public GameObject timeBar;
    public CanvasGroup gameOverGroup;
    private RectTransform barRectTransform;
    public UnityEvent stopGameEvent;

    void Start()
    {
        barRectTransform = timeBar.GetComponent<RectTransform>();
        StartCoroutine(AnimateTimeBar());
    }

    public void ResetBar()
    {
        barRectTransform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(AnimateTimeBar());
    }

    public IEnumerator AnimateTimeBar()
    {
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / gameTime)
        {
            barRectTransform.localScale = new Vector3(Mathf.Lerp(1, 0, t), 1, 1);
            yield return null;
        }

        barRectTransform.localScale = new Vector3(0, 1, 1);

        // Invokes the related methods to stopping the game
        stopGameEvent.Invoke();

        StartCoroutine(ShowGameOverAnimation());
    }

    // Displays the fade-in animation for the game over message
    public IEnumerator ShowGameOverAnimation()
    {
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / 0.8f)
        {
            gameOverGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        gameOverGroup.alpha = 1;
        GameOverFinished();
    }

    // When the game over screen is fully shown
    private void GameOverFinished()
    {
        Time.timeScale = 0f;
        StopAllCoroutines();
    }
}
