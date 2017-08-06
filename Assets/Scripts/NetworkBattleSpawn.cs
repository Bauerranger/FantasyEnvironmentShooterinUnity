using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBattleSpawn : NetworkBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject epicLoot;
    [SerializeField]
    private GameObject spawnFrom;

    [SerializeField]
    private Transform firstWayPoint;



    public void SpawnEpicLoot()
    {
        GameObject spawnedEpicLoot = Instantiate(epicLoot, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(spawnedEpicLoot);
        Destroy(this.gameObject);
    }

    public void SpawnEnemy()
    {
        GameObject spawnedEnemy = Instantiate(enemy, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(spawnedEnemy);
        spawnFrom.GetComponent<BattleManager>().enemysAlive.Add(spawnedEnemy);
        Destroy(this.gameObject);
    }
}
