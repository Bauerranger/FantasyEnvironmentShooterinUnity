using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShots : MonoBehaviour
{
    [SerializeField]
    private int shotUpgradeNo = 0;

    [SerializeField]
    private int shotUpgradeAmmo = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<NetworkShootBehaviors>().powerUpNo = shotUpgradeNo;

            other.GetComponent<NetworkShootBehaviors>().ammoOfPowerUp = shotUpgradeAmmo;
        }
    }
}
