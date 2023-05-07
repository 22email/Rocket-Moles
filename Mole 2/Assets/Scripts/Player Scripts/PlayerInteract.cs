using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float maxDistance; // Max distance from where you can look at mole
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera cam;
    private PlayerUI playerUI; 

    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.updateText(string.Empty);

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * maxDistance);

        if(Physics.Raycast(ray, out hit, maxDistance))
        {
            if(hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

                playerUI.updateText(interactable.promptMessage);    

                if(Input.GetKeyDown(interactKey))
                {
                    interactable.baseInteract();
                }
            }
        }
    }
}
