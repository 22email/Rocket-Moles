using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private CameraController cameraController;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused) pause();

            else unPause();
        }
    }

    public void pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
        cameraController.canMoveMouse = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 

    }

    public void unPause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
        cameraController.canMoveMouse = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }
}
