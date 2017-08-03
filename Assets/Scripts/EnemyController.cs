using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : StatefulMonoBehaviour<EnemyController>
{
    public bool usesRangedWeapons;
    public bool dead;
    public Transform currentWaypoint;
    public int maximumAttackDistance = 8;
    public int seeingDistance = 10;
    public int maximumDistance = 8;
    public int Health = 100;
    private int oldHealth;
    public int deathScore = 25;
    public int enemyDamage = 5;

    void Awake()
    {
        oldHealth = Health;
        fsm = new FSM<EnemyController>();
        if (!usesRangedWeapons)
            fsm.Configure(this, new EnemyPatrol());
        if (usesRangedWeapons)
            fsm.Configure(this, new EnemyWait());
    }

    public void TakeDamage(int damageTaken, string player)
    {
        GetComponent<NetworkEnemyManager>().ProxyCommandTakeDamage(damageTaken, player);
    }

    void selfDestruct()
    {
        dead = true;
    }
}