using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    [SerializeField]
    private List<GameObject> enemySpawns = new List<GameObject>();
    [SerializeField]
    private List<GameObject> LootSpawns = new List<GameObject>();
    [System.NonSerialized]
    public List<GameObject> enemysAlive = new List<GameObject>();
    private bool hasSpawned = false;

    private void Update()
    {
        if (hasSpawned)
        {
            foreach (GameObject enemy in enemysAlive)
            {
                if (enemy.GetComponent<EnemyController>().health <= 0)
                {
                    enemysAlive.Remove(enemy);
                }
            }
            if (enemysAlive.Count <= 0)
            {
                foreach (GameObject spawn in enemySpawns)
                {
                    spawn.GetComponent<NetworkBattleSpawn>().SpawnEpicLoot();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject spawn in enemySpawns)
            {
                spawn.GetComponent<NetworkBattleSpawn>().SpawnEnemy();
            }
            hasSpawned = true;
        }
    }
}