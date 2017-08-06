using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShots : MonoBehaviour
{
    [SerializeField]
    private int shotUpgradeNo = 0;

    [SerializeField]
    private int shotUpgradeAmmo = 0;

    [SerializeField]
    private GameObject effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            other.GetComponent<NetworkShootBehaviors>().ammoOfPowerUp = shotUpgradeAmmo;
            other.GetComponent<NetworkShootBehaviors>().powerUpNo = shotUpgradeNo;

            GameObject spawnedParticle = Instantiate(effect, this.gameObject.transform.position, Quaternion.identity);
            spawnedParticle.GetComponent<SelfDestruct>().selfdestruct_in = 0.5f;
            Destroy(this.gameObject);
        }
    }
}
