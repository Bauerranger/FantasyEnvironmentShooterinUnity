using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform firstWayPoint;
    public void SpawnEnemy()
            {
                GameObject spawnedEnemy = Instantiate(enemy, this.transform.position, Quaternion.identity) as GameObject;
        if (firstWayPoint != null)
        {
            spawnedEnemy.GetComponent<EnemyController>().currentWaypoint = firstWayPoint;
        }
}

}
