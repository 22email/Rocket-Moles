using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [SerializeField] private float bobFrequency; // b
    [SerializeField, Range(0, 0.1f)] private float bobHeight; // a

    [SerializeField, Range(0, 10f)] private float threshhold = 6f;

    private PlayerMovement playerMovement;
    private Rigidbody playerRb;
    private Vector3 defaultPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        
    } 
    
    // Update is called once per frame
    void Update()
    {
        Move();
        ToDefaultPosition();
    }

    void Move()
    {
        if(!playerMovement.Grounded) return;
        if(new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z).magnitude < threshhold) return;

        Vector3 pos = Vector3.zero;

        pos.y += bobHeight * Mathf.Sin(bobFrequency * Time.time);
        pos.x += bobHeight * Mathf.Sin(bobFrequency / 2 * Time.time);

        transform.localPosition += pos;
    }

    void ToDefaultPosition()
    {
        if(transform.localPosition == defaultPosition) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, Time.fixedDeltaTime);  
    }
}
