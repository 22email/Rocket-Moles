using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 mouseSens;

    public Transform player;  

    private float camX;
    private float camY; 
    [HideInInspector] public bool canMoveMouse;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        canMoveMouse = true;
    }

    void LateUpdate()
    {
        if (canMoveMouse)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens.x * 0.02f;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens.y * 0.02f;

            camX -= mouseY;
            camY += mouseX;

            camX = Mathf.Clamp(camX, -90f, 90f);

            transform.rotation = Quaternion.Euler(camX, camY, 0);
            player.rotation = Quaternion.Euler(0, camY, 0); 
        }
        
    }

    public void stopMouseMovement()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        canMoveMouse = false;
    }

    public void startMouseMovement()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        canMoveMouse = true;
    }

}
