using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NormalShot : ShotBase
{
    [SerializeField]
    private int shotDamage = 25;
    [SerializeField]
    private float FireForce = 1000.0f;
    private Rigidbody shotRigidbody;

    [SerializeField]
    private List<GameObject> hitEnemyAnimations = new List<GameObject>();
    [SerializeField]
    private List<GameObject> hitEnvitonmentAnimations = new List<GameObject>();

    void Start()
    {
        shotRigidbody = GetComponent<Rigidbody>();
        shotRigidbody.AddForce(transform.forward * FireForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject hit in hitEnemyAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, this.transform.position, Quaternion.identity) as GameObject;
            }
        }
        else
        {
            foreach (GameObject hit in hitEnvitonmentAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, this.transform.position, Quaternion.identity) as GameObject;
            }
        }

        Destroy(gameObject);
        if (!NetworkServer.active)
        {
            return;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(shotDamage, player);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(this.gameObject);
    }
}
