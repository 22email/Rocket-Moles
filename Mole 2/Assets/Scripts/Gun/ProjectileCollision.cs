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
        if(co.gameObject.tag != "Bullet" && co.gameObject.tag != "Player" && co.gameObject.tag != "Gun" && !collided)
        {
            GameObject explosionObj = Instantiate(explosion, co.contacts[0].point, Quaternion.identity);
            collided = true; 

            doExplosionForce();

            Destroy(explosionObj, 2f);
            Destroy(gameObject);
        }
   }

   void doExplosionForce()
   {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider c in colliders)
        {
            Rigidbody colliderRb = c.GetComponent<Rigidbody>();

            if(colliderRb != null)
            {   
                colliderRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }

        }
   }
}
