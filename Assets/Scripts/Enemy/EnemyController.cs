using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : StatefulMonoBehaviour<EnemyController>
{
    [System.NonSerialized]
    public bool isInStage3;
    public bool isBoss;
    [System.NonSerialized]
    public bool isInBossfight;
    public bool isWaiting;
    public bool isMage;
    [System.NonSerialized]
    public bool dead;
    [System.NonSerialized]
    public GameObject spawnedBy;
    [Space(15)]

    public Transform currentWaypoint;

    [Space(15)]

    public int maximumAttackDistance = 8;
    public int seeingDistance = 10;
    public int maximumDistance = 8;
    public int health = 100;
    private int oldHealth;
    public int deathScore = 25;
    public int enemyDamage = 5;

    [Space(15)]

    [System.NonSerialized]
    public bool isPatroling = false;
    [System.NonSerialized]
    public List<GameObject> playersInReach = new List<GameObject>();
    [System.NonSerialized]
    public bool attacks = false;
    [System.NonSerialized]
    public bool killedPlayer = false;
    public List<GameObject> hitPlayerAnimations;
    public Transform[] BossShotSpawns;

    public List<GameObject> projectilesForBossShot;
    public List<GameObject> effectsForBossShot;
    public List<GameObject> effectsForBossDrop;

    [System.NonSerialized]
    public bool waitBeforeAttack = false;

    private void Start()
    {
        oldHealth = health;
        fsm = new FSM<EnemyController>();
        if (isBoss)
            fsm.Configure(this, new BossWait());
        if (!isWaiting && !isBoss)
            fsm.Configure(this, new EnemyPatrol());
        if (isWaiting && !isBoss)
            fsm.Configure(this, new EnemyWait());
    }
    /// <summary>
    /// Sends the taken damage and the inflicting player to the network. The player is used for the highscore
    /// </summary>
    /// <param name="damageTaken">Damage the projectile inflicts</param>
    /// <param name="player">The shooting player</param>
    public void TakeDamage(int damageTaken, GameObject player)
    {
        GetComponent<NetworkEnemyManager>().ProxyCommandTakeDamage(damageTaken, player);
    }

    /// <summary>
    /// Checks if player is dead and if so removes him from the attack list, so the enemy can find a new victim.
    /// </summary>
    public void UpdatePlayerDead()
    {
        if (playersInReach.Count > 0 && playersInReach[0].GetComponent<NetworkPlayerHealth>().health < 0)
        {
            playersInReach.Remove(playersInReach[0]);
            killedPlayer = true;
        }
    }

    /// <summary>
    /// Damages the player. It is only for the close combat units and when the boss jumps (eg. no projectile is involved)
    /// if boss he can hit multiple players at once instead of just the first one he saw.
    /// </summary>
    public void InflictDamage()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().MakeScreenShake(0.5f);
        if (!isBoss)
        {
            if (playersInReach.Count > 0)
                playersInReach[0].GetComponent<NetworkPlayerHealth>().ReceiveDamage(enemyDamage);
            foreach (GameObject hit in hitPlayerAnimations)
            {
                if (playersInReach.Count > 0)
                {
                    GameObject spawnedParticle = Instantiate(hit, (playersInReach[0].transform.position + new Vector3(0, 1.5f, 0)), Quaternion.identity) as GameObject;
                }
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

    /// <summary>
    /// Tells the BattleManager that this enemy is dead and starts the death events
    /// </summary>
    public void Die()
    {
        if (spawnedBy.GetComponent<BattleManager>().enemysAlive.Count > 0 && spawnedBy.GetComponent<BattleManager>().enemysAlive.Contains(this.gameObject))
            spawnedBy.GetComponent<BattleManager>().enemysAlive.Remove(this.gameObject);
        dead = true;
    }

    /// <summary>
    /// This method is called, when the attack animation ends by the animation. It is intrduced so the enemy only attacks once the attack is done.
    /// </summary>
    public void AttackAnimationEnds()
    {
        attacks = false;
    }

    /// <summary>
    /// This method is called per animation. The boss Jumps and when he drops he inflicts damage on all surrounding players
    /// </summary>
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

    /// <summary>
    /// The boss spawns bunnies that fall down from the sky
    /// </summary>
    public void BigBossAttackSpawn()
    {
        StartCoroutine(spawnParticles());
    }

    public void MakeScreenShake(float shakeTime)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().MakeScreenShake(shakeTime);
    }

    /// <summary>
    /// the enemy has a trigger arround him that adds a player that is in reach to a list. With this method two or even more players can be observed on the same time
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<NetworkPlayerHealth>().health > 0)
            playersInReach.Add(other.gameObject);
    }
    /// <summary>
    /// the enemy has a trigger arround him that deletes the leaving player from a list. With this method two or even more players can be observed on the same time
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playersInReach.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// The Boss spawns his bunnybombs from the shotspawns
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// The third stage of the boss fight is a mix of the second and first boss stage. rather than writing a new stage he changes between stage one and stage two within a certain timespan
    /// </summary>
    /// <param name="state"></param>
    /// <param name="waitTime"></param>
    public void DelayChangeState(string state, float waitTime)
    {
        StartCoroutine(DelayChangeStateEnum(state, waitTime));
    }

    IEnumerator DelayChangeStateEnum(string state, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        string newState = (state);
        GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, gameObject);
    }

    /// <summary>
    /// Basically a bugfix (enemy in the beginning always goes to attack because the distance to its target is always zero in the beginning
    /// </summary>
    public void StartWaitForAttack()
    {
        StartCoroutine(WaitBeforeAttackWaiter());
    }
    /// <summary>
    /// Has to wait before it can change to the attack state, otherweise it would attack immediately
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeAttackWaiter()
    {
        yield return new WaitForSeconds(2);
        waitBeforeAttack = true;
    }
}