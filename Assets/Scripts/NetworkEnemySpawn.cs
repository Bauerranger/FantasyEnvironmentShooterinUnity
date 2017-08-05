using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEnemySpawn : MonoBehaviour {
    //somehow, if this is used, the spawned enemys do not work properly (get no damage/ don´t change states etc.)
    List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
	void Start () {
        enemySpawns.AddRange(GetComponentsInChildren<EnemySpawn>());
	}

    private void OnTriggerEnter(Collider other)
    {
        foreach (EnemySpawn spawn in enemySpawns)
        {
            spawn.SpawnEnemy();
        }
    }

}
