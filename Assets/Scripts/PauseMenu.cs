using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public UnityEvent onPause;
    public UnityEvent onResume;
    public GameObject pauseMenu;

    // Used to prevent pausing while in the game over screen
    private bool canPause = true;
    public bool CanPause
    {
        get { return canPause; }
        set { canPause = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        onPause.Invoke();
        pauseMenu.SetActive(true);

        // foreach(Button b in childButtons)
        // {
        //     b.interactable = true;
        // }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        onResume.Invoke();
        pauseMenu.SetActive(false);

        // foreach(Button b in childButtons)
        // {
        //     b.interactable = false;
        // }
    }
}
