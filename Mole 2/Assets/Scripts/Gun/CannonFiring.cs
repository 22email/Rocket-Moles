using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFiring : MonoBehaviour
{
    public Camera cam; 
    public Transform shootPoint;
    public  GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;
    [SerializeField] private float shootForce;
    [SerializeField] private float shootDelay;
    private Vector3 destination;
    private bool canShoot;

    private GunRecoil gunRecoil;

    public bool CanShoot
    {
        get{return canShoot;}
        set{canShoot = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        gunRecoil = GetComponent<GunRecoil>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && canShoot)
        {
            canShoot = false;
            shootProj();  

            Invoke("resetShoot", shootDelay);
        }
    }

    private void shootProj() 
    {
        muzzleFlash.Play();
        gunRecoil.doRecoil();

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) 
            destination = hit.point;    
        
        else
            destination = ray.GetPoint(1000);

        Vector3 shootDirection = destination - shootPoint.position;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.transform.forward = shootDirection.normalized;

        bullet.GetComponent<Rigidbody>().velocity = shootDirection.normalized * shootForce;

        Destroy(bullet, 2f);

        
    }

    private void resetShoot()
    {
        canShoot = true;
    }

}
