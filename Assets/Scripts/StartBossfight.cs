using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossfight : MonoBehaviour {

    public GameObject gameManager;
    public GameObject boss;
    private int count;

    private void OnTriggerExit(Collider other)
    {
        gameManager.GetComponent<GameManagerScript>().inBossfight = true;
        boss.GetComponent<EnemyController>().isInBossfight = true;
        count++;
        if (count >= 2)
        GetComponent<BoxCollider>().enabled = true;
    }
}
