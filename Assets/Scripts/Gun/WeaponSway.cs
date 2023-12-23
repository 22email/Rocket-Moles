// From https://youtu.be/QIVN-T-1QBE
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField]
    private float multiplier;

    [SerializeField]
    private float smooth;

    private bool canSway;

    public bool CanSway
    {
        get => canSway;
        set => canSway = value;
    }

    void Awake()
    {
        // TODO: Retrive this value from a saved JSON file
        canSway = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSway)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            smooth * Time.deltaTime
        );
    }
}
