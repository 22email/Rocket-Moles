using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameLoopController : MonoBehaviour
{
    [SerializeField]
    private float gameTime;

    public GameObject timeBar;
    public CanvasGroup gameOverGroup;

    private RectTransform barRectTransform;

    public UnityEvent stopGameEvent;
    public UnityEvent restartGameEvent;

    private GameObject player;

    void Awake()
    {
        Application.targetFrameRate = -1;
    }

    void Start()
    {
        barRectTransform = timeBar.GetComponent<RectTransform>();
        StartCoroutine(StartGameLoop());
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RestartGame()
    {
        // Destroy all bullets
        var existingBullets = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject bullet in existingBullets)
        {
            Destroy(bullet);
        }

        barRectTransform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(StartGameLoop());
        restartGameEvent.Invoke();
        Time.timeScale = 1f;
    }

    // Also controls the time bar animation
    private IEnumerator StartGameLoop()
    {
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / gameTime)
        {
            barRectTransform.localScale = new Vector3(Mathf.Lerp(1, 0, t), 1, 1);
            yield return null;
        }

        barRectTransform.localScale = new Vector3(0, 1, 1);

        // Invokes the related methods (in the event) to stopping the game
        stopGameEvent.Invoke();
        StartCoroutine(ShowGameOverAnimation());
        Time.timeScale = 0f;
    }

    // Displays the fade-in animation for the game over message
    private IEnumerator ShowGameOverAnimation()
    {
        for (float t = 0.0f; t < 1f; t += Time.fixedDeltaTime / 0.8f)
        {
            gameOverGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        gameOverGroup.alpha = 1;
        GameOverAnimationFinished();
    }

    // When the game over screen is fully shown
    private void GameOverAnimationFinished()
    {
        StopAllCoroutines();
    }
}
