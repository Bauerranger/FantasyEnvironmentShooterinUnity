using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleManager : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> enemySpawns = new List<GameObject>();
    [SerializeField]
    private List<GameObject> twoPlayerEnemySpawns = new List<GameObject>();
    [SerializeField]
    private List<GameObject> enemySpawnWaves = new List<GameObject>();
    [SerializeField]
    private List<GameObject> lootSpawns = new List<GameObject>();
    [SerializeField]
    private List<GameObject> twoPlayerLootSpawns = new List<GameObject>();
    [System.NonSerialized]
    public List<GameObject> enemysAlive = new List<GameObject>();
    private bool hasSpawned = false;
    private bool hasSpwanedLoot = false;
    private int playerCount = 0;
    public int waveCount = 0;
    public float waitTime = 0;
    private GameObject playerTriggered;

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
                if (!hasSpwanedLoot)
                {
                    foreach (GameObject spawn in lootSpawns)
                    {
                        spawn.GetComponent<NetworkBattleSpawn>().SpawnEpicLoot();
                    }
                    if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
                    {
                        foreach (GameObject spawn in twoPlayerLootSpawns)
                        {
                            spawn.GetComponent<NetworkBattleSpawn>().SpawnEpicLoot();
                        }
                    }
                    hasSpwanedLoot = true;
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
            if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
            {
                foreach (GameObject spawn in twoPlayerEnemySpawns)
                {
                    spawn.GetComponent<NetworkBattleSpawn>().SpawnEnemy();
                }
            }
            if (isServer)
            {
                StartCoroutine(SpawnWaves());
            }
            hasSpawned = true;
            if (other.gameObject == playerTriggered)
                playerCount++;
            if (other.gameObject != playerTriggered)
                playerCount--;
            if (playerCount >= GameObject.FindGameObjectsWithTag("Player").Length)
            {
                colliderStart.enabled = true;
            }
            playerTriggered = other.gameObject;
        }
    }

    IEnumerator SpawnWaves()
    {
        for (int wavesSpawned = 0; wavesSpawned < waveCount; wavesSpawned++)
        {
            foreach (GameObject spawn in enemySpawnWaves)
            {
                spawn.GetComponent<NetworkBattleSpawn>().SpawnEnemy();
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}