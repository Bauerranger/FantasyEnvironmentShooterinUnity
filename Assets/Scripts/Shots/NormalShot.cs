using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NormalShot : ShotBase
{
    public int shotDamage = 25;
    public float FireForce = 1000.0f;
    public List<GameObject> hitEnemyAnimations = new List<GameObject>();
    public List<GameObject> hitEnvironmentAnimations = new List<GameObject>();
    public List<GameObject> hitPlayerAnimations = new List<GameObject>();
    public Rigidbody shotRigidbody;

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
            collision.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
        }
        else if (collision.gameObject.tag == "Player")
        {
            foreach (GameObject hit in hitPlayerAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, this.transform.position, Quaternion.identity) as GameObject;
            }
            collision.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
        }
        else
        {
            foreach (GameObject hit in hitEnvironmentAnimations)
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

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<NetworkPlayerHealth>().ReceiveDamage(shotDamage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(this.gameObject);
    }
}
