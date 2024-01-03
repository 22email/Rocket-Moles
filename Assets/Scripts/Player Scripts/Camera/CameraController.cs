using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float mouseSens;

    public Transform player;

    private float camX;
    private float camY;

    private bool canLook;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Confined)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * 0.02f;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * 0.02f;

        camX -= mouseY;
        camY += mouseX;

        camX = Mathf.Clamp(camX, -90f, 90f);

        transform.rotation = Quaternion.Euler(camX, camY, 0);
        player.rotation = Quaternion.Euler(0, camY, 0);
    }

    // Methods used by events

    // Make the player inable to look around but allows free mouse movement
    public void disallowLook() => Cursor.lockState = CursorLockMode.Confined; // Cursor.visible = true;// canLook = false;

    // Make the player able to look around
    public void allowLook() => Cursor.lockState = CursorLockMode.Locked; // Cursor.visible = false;// canLook = true;
}
