using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;

    [SerializeField]
    private float shootForce;

    [SerializeField]
    private float shootDelay;
    private Vector3 destination;

    private GunRecoil gunRecoil;

    private bool canFire;

    public bool CanFire
    {
        get => canFire;
        set => canFire = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        gunRecoil = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunRecoil>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canFire)
        {
            canFire = false;
            shootProjectile();

            Invoke(nameof(ResetShoot), shootDelay);
        }
    }

    private void ResetShoot() => canFire = true;

    private void shootProjectile()
    {
        muzzleFlash.Play();
        gunRecoil.DoRecoil();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
            destination = hit.point;
        else
            destination = ray.GetPoint(1000);

        Vector3 shootDirection = destination - shootPoint.position;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.transform.forward = shootDirection.normalized;

        bullet.GetComponent<Rigidbody>().velocity = shootDirection.normalized * shootForce;

        Destroy(bullet, 2f);
    }

}
