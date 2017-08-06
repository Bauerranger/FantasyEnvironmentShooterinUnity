using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour {

    [SerializeField]
    private int shotDamage = 5;
    [SerializeField]
    private List<GameObject> hitAnimations = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<NetworkPlayerHealth>().ReceiveDamage(shotDamage);
            other.GetComponent<Animator>().SetTrigger("Take Damage");
        }
        foreach (GameObject hit in hitAnimations)
        {
            GameObject spawnedParticle = Instantiate(hit, this.transform.position, Quaternion.identity) as GameObject;
        }
        if (other.tag == "Player" || other.tag == "floor" || other.tag == "PlayerShot" || other.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
