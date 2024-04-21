// Attatch this script to the main camera
// from https://www.youtube.com/watch?v=5MbR2qJK8Tc

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float cameraBobHeight;
    [SerializeField] private float cameraBobFrequency;

    [Header("Weapons")]
    [SerializeField] private float weaponBobHeight = 0.002f;
    [SerializeField] private float weaponBobFrequency = 6f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject weaponsHolder;
    
    private float threshhold = 6f;
    private bool canBob = true;

    public bool CanBob
    {
        get { return canBob; }
        set { canBob = value; }
    }

    private PlayerMovement playerMovement;
    private Rigidbody playerRb;

    private Vector3 defaultPosition = Vector3.zero; // always at 0 0 0 

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canBob) return;

        Move(gameObject, cameraBobHeight, cameraBobFrequency); // Move camera
        Move(weaponsHolder, weaponBobHeight, weaponBobFrequency); // Move weapons

        FixAiming(); 

        MoveToDefaultPosition(gameObject);
        MoveToDefaultPosition(weaponsHolder);
        
    }

    // Method only used for the camera
    void FixAiming()
    {
        transform.LookAt(new Vector3(player.position.x, player.position.y + cameraHolder.localPosition.y, player.position.z) + cameraHolder.forward * 15f);
        Vector3 clampedRotation = transform.localRotation.eulerAngles;

        // Clamps the z rotation to a small value to fix a bug where the camera would flip upside down everytime the player looked directly up or down
        clampedRotation.z = (clampedRotation.z > 180) ? clampedRotation.z - 360 : clampedRotation.z;
        clampedRotation.z = Mathf.Clamp(clampedRotation.z, -0.001f, 0.001f);

        transform.localRotation = Quaternion.Euler(clampedRotation);
    }
    
    void Move(GameObject objToMove, float a, float b)
    {
        if(!playerMovement.Grounded) return;
        if(new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z).sqrMagnitude< threshhold * threshhold) return;

        Vector3 pos = Vector3.zero;

        pos.y += a * Mathf.Sin(b * Time.time);
        pos.x += (objToMove == weaponsHolder) ? a * Mathf.Sin(b / 2 * Time.time) : a * Mathf.Sin( 1.2f * b * Time.time);

        objToMove.transform.localPosition += pos;
        
    }

    void MoveToDefaultPosition(GameObject objToMove)
    {
        if(objToMove.transform.localPosition == defaultPosition) return;

        objToMove.transform.localPosition = Vector3.Lerp(objToMove.transform.localPosition, defaultPosition, Time.deltaTime);
    }
}
