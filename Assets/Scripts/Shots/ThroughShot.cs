using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThroughShot : NormalShot
{
    private void Start()
    {
        shotRigidbody.AddForce(transform.forward * FireForce);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        {
            if (collision.gameObject.tag == "Enemy")
            {
                foreach (GameObject hit in hitEnemyAnimations)
                {
                    GameObject spawnedParticle = Instantiate(hit, collision.transform.position, Quaternion.identity) as GameObject;
                }
                collision.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
            }

            if (!NetworkServer.active)
            {
                return;
            }

            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(shotDamage, player);
            }
        }
    }
}
