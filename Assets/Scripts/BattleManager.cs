using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemySpawns = new List<GameObject>();
    [SerializeField]
    private List<GameObject> LootSpawns = new List<GameObject>();
    [System.NonSerialized]
    public List<GameObject> enemysAlive = new List<GameObject>();
    private bool hasSpawned = false;
    private int count = 0;

    [SerializeField]
    private Collider colliderStart;
    [SerializeField]
    private Collider colliderEnd;

    private void Update()
    {
        if (hasSpawned)
        {
            if (enemysAlive.Count <= 0)
            {
                colliderEnd.enabled = false;

                foreach (GameObject spawn in LootSpawns)
                {
                    spawn.GetComponent<NetworkBattleSpawn>().SpawnEpicLoot();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasSpawned)
        {
            foreach (GameObject spawn in enemySpawns)
            {
                spawn.GetComponent<NetworkBattleSpawn>().SpawnEnemy();
            }
            hasSpawned = true;
            count++;
            if (count >= GameObject.FindGameObjectsWithTag("Player").Length)
            {
                colliderStart.enabled = true;
            }
        }
    }
}