using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A shot fired into the air, hits multiple enemys until it is destroyed by a destroy by time script (from the hits and slashes asset pack)
/// </summary>
public class MortarShot : NormalShot
{
    /// <summary>
    /// overwrites the Normalshot OnCollisionEnter, so the projectile does not destroy itself on collision, but rather has the ability to hit more than one enemy.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(shotDamage, player);
            foreach (GameObject hit in hitEnemyAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, collision.transform.position, Quaternion.identity) as GameObject;
            }
            collision.gameObject.GetComponent<Animator>().SetTrigger("Take Damage");
        }
    }

    /// <summary>
    /// Plays an animation when destroyed by an other script
    /// </summary>
    private void OnDestroy()
    {
        foreach (GameObject hit in hitEnvironmentAnimations)
        {
            GameObject spawnedParticle = Instantiate(hit, this.transform.position, Quaternion.identity) as GameObject;
        }
    }

}
