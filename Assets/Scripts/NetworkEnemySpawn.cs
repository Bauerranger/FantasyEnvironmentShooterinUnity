using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEnemySpawn : MonoBehaviour {

    List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
	void Start () {
        enemySpawns.AddRange(GetComponentsInChildren<EnemySpawn>());
	}


}
