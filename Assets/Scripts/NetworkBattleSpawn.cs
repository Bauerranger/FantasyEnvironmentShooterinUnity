using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBattleSpawn : NetworkBehaviour
{
    [SerializeField]
    private EnemyController enemy;
    [SerializeField]
    private GameObject epicLoot;
    [SerializeField]
    private GameObject spawnFrom;

    [SerializeField]
    private Transform firstWayPoint;



    public void SpawnEpicLoot()
    {
        if (!isServer)
            return;
        GameObject spawnedEpicLoot = Instantiate(epicLoot, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(spawnedEpicLoot);
        Destroy(this.gameObject);
    }

    public void SpawnEnemy()
    {
        if (!isServer)
            return;
        EnemyController spawnedEnemy = Instantiate(enemy, this.transform.position, this.transform.rotation);
        NetworkServer.Spawn(spawnedEnemy.gameObject);
        spawnFrom.GetComponent<BattleManager>().enemysAlive.Add(spawnedEnemy.gameObject);
        spawnedEnemy.GetComponent<EnemyController>().spawnedBy = spawnFrom;
        if (firstWayPoint)
        {
            Cmd_ChangeWayPoint(spawnedEnemy.gameObject);
        }
    }

    [Command]
    void Cmd_ChangeWayPoint(GameObject spawnedEnemy)
    {
        Rpc_ChangeWayPoint(spawnedEnemy);
    }

    [ClientRpc]
    void Rpc_ChangeWayPoint(GameObject spawnedEnemy)
    {
        spawnedEnemy.GetComponent<EnemyController>().currentWaypoint = firstWayPoint;
    }
}
