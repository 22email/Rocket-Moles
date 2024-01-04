using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public ParticleSystem muzzleFlash;

    [SerializeField]
    private float shootForce;

    [SerializeField]
    private float shootDelay;

    [SerializeField]
    private AudioSource shootSound;

    private Vector3 destination;
    private GunRecoil gunRecoil;
    private bool canShoot;

    public bool CanShoot
    {
        get => canShoot;
        set => canShoot = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        gunRecoil = GameObject.FindGameObjectWithTag("Gun").GetComponent<GunRecoil>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            canShoot = false;
            shootProjectile();

            shootSound.pitch = Random.Range(0.8f, 1f);
            shootSound.Play();
            Invoke(nameof(ResetShoot), shootDelay);
        }
    }

    private void ResetShoot() => canShoot = true;

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
