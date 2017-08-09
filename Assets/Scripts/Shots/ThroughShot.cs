using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Shot that shoots through enemys and damages more than one entity
/// </summary>
public class ThroughShot : NormalShot
{
    /// <summary>
    /// initial force for the shot trigger
    /// </summary>
    private void Start()
    {
        shotRigidbody.AddForce(transform.forward * FireForce);
    }

    /// <summary>
    /// overwrite the collision to make it a trigger dependent shot
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

    }

    /// <summary>
    /// Shot should follow player lika a flamethrower
    /// </summary>
    private void Update()
    {
        transform.position = player.transform.position + new Vector3(1, 1, 0);
    }

    /// <summary>
    /// Makes enemy take damage
    /// </summary>
    /// <param name="collision">other entity hit by the trigger</param>
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
