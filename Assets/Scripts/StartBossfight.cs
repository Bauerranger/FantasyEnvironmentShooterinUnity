using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossfight : MonoBehaviour {

    public GameObject gameManager;
    public GameObject boss;
    private int count;

    /// <summary>
    /// Starts boss fight
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        gameManager.GetComponent<GameManagerScript>().inBossfight = true;
        boss.GetComponent<EnemyController>().isInBossfight = true;
        count++;
        if (count >= GameObject.FindGameObjectsWithTag("Player").Length)
            GetComponent<BoxCollider>().enabled = true;
    }
}
