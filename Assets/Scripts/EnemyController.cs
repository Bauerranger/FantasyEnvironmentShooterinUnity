using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : StatefulMonoBehaviour<EnemyController>
{
    public bool isBoss;
    public bool isInBossfight;
    public bool usesRangedWeapons;
    public bool isMage;
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
    public List<GameObject> hitPlayerAnimations;
    public Transform[] BossWayPoints;
    public Transform[] BossShotSpawns;

    public List<GameObject> projectilesForBossShot;
    public List<GameObject> effectsForBossShot;
    public List<GameObject> effectsForBossDrop;


    void Awake()
    {
        oldHealth = health;

    }

    private void Start()
    {
        fsm = new FSM<EnemyController>();
        if (isBoss)
            fsm.Configure(this, new BossWait());
        if (!usesRangedWeapons && !isBoss)
            fsm.Configure(this, new EnemyPatrol());
        if (usesRangedWeapons && !isBoss)
            fsm.Configure(this, new EnemyWait());
    }

    public void TakeDamage(int damageTaken, GameObject player)
    {
        GetComponent<NetworkEnemyManager>().ProxyCommandTakeDamage(damageTaken, player);
    }

    public void UpdatePlayerDead()
    {
        if (playersInReach.Count > 0 && playersInReach[0].GetComponent<NetworkPlayerHealth>().health < 0)
        {
            playersInReach.Remove(playersInReach[0]);
            killedPlayer = true;
        }
    }

    public void InflictDamage()
    {
        if (!isBoss)
        {
            if (playersInReach.Count > 0)
                playersInReach[0].GetComponent<NetworkPlayerHealth>().ReceiveDamage(enemyDamage);
            foreach (GameObject hit in hitPlayerAnimations)
            {
                GameObject spawnedParticle = Instantiate(hit, (playersInReach[0].transform.position + new Vector3(0, 1.5f, 0)), Quaternion.identity) as GameObject;
            }
        }
        if (isBoss)
        {

            foreach (GameObject player in playersInReach)
            {
                player.GetComponent<NetworkPlayerHealth>().ReceiveDamage(enemyDamage);
                player.GetComponent<Animator>().SetTrigger("Take Damage");
                foreach (GameObject hit in hitPlayerAnimations)
                {
                    GameObject spawnedParticle = Instantiate(hit, (player.transform.position + new Vector3(0, 1.5f, 0)), Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void SelfDestruct()
    {
        dead = true;
    }

    public void AttackAnimationEnds()
    {
        attacks = false;
    }

    public void BossAttack()
    {
        foreach (GameObject player in playersInReach)
        {
            InflictDamage();
        }
        foreach (GameObject effect in effectsForBossDrop)
        {
            GameObject dropEffect = Instantiate(effect, this.transform.position, this.transform.rotation);
        }
    }

    public void BigBossAttackSpawn()
    {
            StartCoroutine(spawnParticles());
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

    IEnumerator spawnParticles()
    {
        foreach (Transform spawnPoint in BossShotSpawns)
        {
            foreach (GameObject effect in effectsForBossShot)
            {
                GameObject effectAtSpawn = Instantiate(effect, spawnPoint.position, spawnPoint.rotation);
            }
            float random = Random.Range(0, 3);
            if (random >= 0 && random < 1)
            {
                GameObject bossShot = Instantiate(projectilesForBossShot[0], spawnPoint.position, spawnPoint.rotation);
            }
            else if (random >= 1 && random < 2)
            {
                GameObject bossShot = Instantiate(projectilesForBossShot[1], spawnPoint.position, spawnPoint.rotation);
            }
            else if (random >= 2 && random < 4)
            {
                GameObject bossShot = Instantiate(projectilesForBossShot[2], spawnPoint.position, spawnPoint.rotation);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}