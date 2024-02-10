using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private float explosionForce = 7f;

    [SerializeField]
    private float explosionRadius = 10f;

    private bool collided;

    void OnCollisionEnter(Collision co)
    {
        if (!co.gameObject.CompareTag("Projectile") && !collided)
        {
            GameObject explosionObj = Instantiate(
                explosion,
                co.contacts[0].point,
                Quaternion.identity
            );

            // Don't need to call Play() since the source plays on awake
            AudioSource explosionSound = explosionObj.GetComponent<AudioSource>();
            explosionSound.pitch = Random.Range(0.8f, 1f);

            collided = true;

            DoExplosionForce();
            Destroy(explosionObj, 2f);
            Destroy(gameObject);
        }
    }

    void DoExplosionForce()
    {
        Collider[] colliders = new Collider[10];

        // The amount of colliders that exist within the explosion radius
        var count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders);

        foreach (Collider c in colliders)
        {
            if (c != null && c.TryGetComponent<Rigidbody>(out var colliderRb))
            {
                if (
                    colliderRb.TryGetComponent<PlayerMovement>(out var pm)
                    && (pm.transform.position - transform.position).sqrMagnitude < 15
                )
                {
                    pm.MoveToSlope = false;
                    pm.MoveSpeed = pm.DefaultMoveSpeed * 8f;

                    pm.StopAllCoroutines();
                    pm.StartCoroutine(pm.SlowDown());
                    pm.StartCoroutine(pm.FallToGround());
                }

                colliderRb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius,
                    1f,
                    ForceMode.Impulse
                );
            }
        }
    }
}
