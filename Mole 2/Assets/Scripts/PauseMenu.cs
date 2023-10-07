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
    private bool isPaused = false;

    private bool canPause = true;
    public bool CanPause 
    {
        get { return canPause; }
        set { canPause = value; }
    }

    private bool canUnPause = true;

    public bool CanUnPause {get => canPause; set => canUnPause = value;}

    private Button[] childButtons;

    // Start is called before the first frame update
    void Start()
    {
        childButtons = pauseMenu.GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if(!isPaused) Pause();

            else {
                if (canUnPause) Resume();
            }
            
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        onPause.Invoke();

        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

        // foreach(Button b in childButtons) 
        // {
        //     b.interactable = false;
        // }
    }
}
