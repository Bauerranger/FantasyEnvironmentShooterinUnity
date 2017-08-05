using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossfight : MonoBehaviour {

    public GameObject gameManager;
    public GameObject boss;

    private void OnTriggerExit(Collider other)
    {
        gameManager.GetComponent<GameManagerScript>().inBossfight = true;
        boss.GetComponent<EnemyController>().isInBossfight = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
