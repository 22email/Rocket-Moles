using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] private GameObject bar;
    [SerializeField] private CanvasGroup gameOverGroup;
    [SerializeField] private Button playButton;
    private RectTransform barRectTransform;
    public UnityEvent stopGameEvent;
    
    
    // Start is called before the first frame update
    void Start()
    {
        barRectTransform = bar.GetComponent<RectTransform>();
        StartCoroutine(animateTimeBar()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetBar()
    {
        barRectTransform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(animateTimeBar()); 
    }

    public IEnumerator animateTimeBar()
    {
        for(float t = 0.0f; t < 1f; t += Time.deltaTime / gameTime)
        {
            barRectTransform.localScale = new Vector3(Mathf.Lerp(1, 0, t), 1, 1);
            yield return null;
        }

        barRectTransform.localScale = new Vector3(0, 1, 1);
        StartCoroutine(showGameOverScreen());
    }

    public IEnumerator showGameOverScreen()
    {
        stopGameEvent.Invoke();

        for(float t = 0.0f; t < 1f; t += Time.deltaTime / 0.5f)
        {
            gameOverGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        gameOverGroup.alpha = 1;
        stopGame();
    }

    private void stopGame() 
    {
        playButton.interactable = true;
        Time.timeScale = 0f;
        StopAllCoroutines();
        
    }
}
