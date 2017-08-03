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
    public int health = 100;
    private int oldHealth;
    public int deathScore = 25;
    public int enemyDamage = 5;
    public bool isPatroling = false;
    public List<GameObject> playersInReach = new List<GameObject>();
    public bool attacks = false;
    public bool killedPlayer = false;

    void Awake()
    {
        oldHealth = health;
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

    public void UpdatePlayerDead()
    {
        if (playersInReach.Count > 0 && playersInReach[0].GetComponent<NetworkPlayerHealth>().health  < 0)
        {
            playersInReach.Remove(playersInReach[0]);
            killedPlayer = true;
        }
    }

    public void InflictDamage()
    {
        if(playersInReach[0])
        playersInReach[0].GetComponent<NetworkPlayerHealth>().ReceiveDamage(enemyDamage);
    }

    void SelfDestruct()
    {
        dead = true;
    }

    public void AttackAnimationEnds()
    {
        attacks = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<NetworkPlayerHealth>().health > 0)
            playersInReach.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playersInReach.Remove(other.gameObject);
        }
    }
}