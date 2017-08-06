using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : MonoBehaviour {
    [SerializeField]
    private int addLife = 25;
    [SerializeField]
    private GameObject effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<NetworkPlayerHealth>().health += addLife;
            GameObject spawnedParticle = Instantiate(effect, this.gameObject.transform.position, Quaternion.identity);
            spawnedParticle.GetComponent<SelfDestruct>().selfdestruct_in = 0.5f;
            Destroy(this.gameObject);
        }
    }
}
