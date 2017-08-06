using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShot : NormalShot {

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

    private void OnDestroy()
    {
        foreach (GameObject hit in hitEnvironmentAnimations)
        {
            GameObject spawnedParticle = Instantiate(hit, this.transform.position, Quaternion.identity) as GameObject;
        }
    }

}
