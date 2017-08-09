using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A shot that shoots three times as fast as the normal shot
/// </summary>
public class FastShot : NormalShot
{

    [SerializeField]
    private GameObject[] shots;
    [SerializeField]
    private float waitTime = 0.01f;

    /// <summary>
    /// three shots are spawned instantly but get their initial speed after a waitTime
    /// </summary>
    void Start()
    {
        StartCoroutine(WaitBetweenShots(waitTime));
    }

    /// <summary>
    /// Since the shot prefab itself does not collide with the enemy, this method handles the point when the enemy is hit by a projectile and gives Damage, spawns effects and sets the tage damage trigger
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// Waits to give initial speedup to the three projectiles.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
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
