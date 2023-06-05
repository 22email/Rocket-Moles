// https://www.youtube.com/watch?v=W0AZ-owxCog

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Camera cam;

    private Vector3 currentRotation; 
    private Vector3 targetRotation; 

    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private Vector3 initialGunPosition;  

    [SerializeField] private Vector3 defaultRotation;
    [SerializeField] private Vector3 recoil;

    [SerializeField] private float kickbackZ;

    [SerializeField] private float returnAmount;
    [SerializeField] private float snappiness;

    void Start()
    {
        initialGunPosition = transform.localPosition;
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, defaultRotation, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRotation);

        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = currentPosition; 
        
    }

    public void doRecoil()
    {
        targetPosition -= new Vector3(0, 0, kickbackZ);
        targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
    }
}

