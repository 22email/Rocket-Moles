using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    private bool collided;
    public GameObject explosion;
    public float explosionForce;
    public float explosionRadius;

    void OnCollisionEnter(Collision co)
    {
        if (
            !co.gameObject.CompareTag("Projectile")
            && !co.gameObject.CompareTag("Player")
            && !co.gameObject.CompareTag("Gun")
            && !collided
        )
        {
            GameObject explosionObj = Instantiate(
                explosion,
                co.contacts[0].point,
                Quaternion.identity
            );

            var audioSource = explosionObj.GetComponent<AudioSource>();
            audioSource.pitch = Random.Range(0.8f, 1f);
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
                if (colliderRb.TryGetComponent<PlayerMovement>(out var pm))
                {
                    pm.MoveToSlope = false;
                    pm.MoveSpeed = pm.DefaultMoveSpeed * 5.2f;

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
