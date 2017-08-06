using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FastShot : NormalShot
{

    [SerializeField]
    private GameObject[] shots;
    [SerializeField]
    private float waitTime = 0.01f;

    void Start()
    {
        StartCoroutine(WaitBetweenShots(waitTime));
    }

    public void Collide(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject hit in hitEnemyAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, collision.transform.position, Quaternion.identity) as GameObject;
            }
            collision.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
        }
        else
        {
            foreach (GameObject hit in hitEnvironmentAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, collision.transform.position, Quaternion.identity) as GameObject;
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

    IEnumerator WaitBetweenShots(float waitTime)
    {
        foreach (GameObject shot in shots)
        {
            shotRigidbody = shot.GetComponent<Rigidbody>();
            shotRigidbody.AddForce(transform.forward * FireForce);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
