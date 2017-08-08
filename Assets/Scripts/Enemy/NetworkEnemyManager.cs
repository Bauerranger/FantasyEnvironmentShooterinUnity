using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;


public class NetworkEnemyManager : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> deathParticles = new List<GameObject>();
    [SerializeField]
    private ShotBase shotPrefab;
    public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.enabled = isServer;
    }

    void SelfDestruct()
    {
        if (!isServer)
            return;
        GetComponent<EnemyController>().Die();
    }

    public void ProxyCommandSetRunBool(bool run)
    {
        Cmd_SetRunBool(run);
    }

    [Command]
    void Cmd_SetRunBool(bool run)
    {
        Rpc_SetRunBool(run);
    }

    [ClientRpc]
    void Rpc_SetRunBool(bool run)
    {
        GetComponent<Animator>().SetBool("Run", run);
    }

    public void ProxyCommandChangeDestination(GameObject destination)
    {
        Cmd_ChangeDestination(destination.transform.position);
    }

    [Command]
    void Cmd_ChangeDestination(Vector3 destination)
    {
        Rpc_ChangeDestination(destination);
    }

    [ClientRpc]
    void Rpc_ChangeDestination(Vector3 destination)
    {
        if (agent.isActiveAndEnabled)
            agent.destination = destination;
    }

    public void ProxyCommandTakeDamage(int damageTaken, GameObject player)
    {
        Cmd_TakeDamage(damageTaken, player.GetComponent<NetworkIdentity>().netId.ToString());
    }

    [Command]
    void Cmd_TakeDamage(int damageTaken, string player)
    {
        Rpc_TakeDamage(damageTaken, player);
    }

    [ClientRpc]
    void Rpc_TakeDamage(int damageTaken, string player)
    {
        this.GetComponent<EnemyController>().health -= damageTaken;
        GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ProxyCommandAddScore(player, damageTaken);
        if (this.GetComponent<EnemyController>().health <= 0)
            GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ProxyCommandAddScore(player, this.GetComponent<EnemyController>().deathScore);
    }

    public void ProxyCommandChangeState(string stateName, GameObject player)
    {
        Cmd_ChangeState(stateName, player);
    }

    [Command]
    void Cmd_ChangeState(string stateName, GameObject player)
    {
        Rpc_ChangeState(stateName, player);
    }

    [ClientRpc]
    void Rpc_ChangeState(string stateName, GameObject player)
    {
        switch (stateName)
        {
            case "EnemyChase":
                this.GetComponent<EnemyController>().ChangeState(new EnemyChase(player));
                break;

            case "EnemyPatrol":
                this.GetComponent<EnemyController>().ChangeState(new EnemyPatrol());
                break;

            case "EnemyAttack":
                this.GetComponent<EnemyController>().ChangeState(new EnemyAttack(player));
                break;

            case "EnemyWait":
                this.GetComponent<EnemyController>().ChangeState(new EnemyWait());
                break;

            case "EnemyDead":
                this.GetComponent<EnemyController>().ChangeState(new EnemyDead());
                break;

            case "BossAttack":
                this.GetComponent<EnemyController>().ChangeState(new BossAttack());
                break;

            case "BossStage2":
                this.GetComponent<EnemyController>().ChangeState(new BossStage2());
                break;

            case "BossDead":
                this.GetComponent<EnemyController>().ChangeState(new BossDead());
                break;

            default:
                break;
        }
    }

    public void ProxyCommandDie()
    {
        Cmd_Die();
    }

    [Command]
    void Cmd_Die()
    {
        Rpc_SpawnDeath();
    }

    [ClientRpc]
    void Rpc_SpawnDeath()
    {
        foreach (GameObject particle in deathParticles)
        {
            GameObject spawnedDeathParticles = Instantiate(particle, this.gameObject.transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    public void ProxyCommandShootProjectile()
    {
        if (gameObject.GetComponent<EnemyController>().playersInReach.Count > 0)
        {
            Vector3 target = gameObject.GetComponent<EnemyController>().playersInReach[0].GetComponent<NetworkShootBehaviors>().ShootStartPoint.position;
            Cmd_ShootProjectile(target);
        }
    }

    [Command]
    void Cmd_ShootProjectile(Vector3 target)
    {
        Vector3 shotStartPoint = this.transform.position + new Vector3(0f, 1f, 0f);
        Quaternion rotation = Quaternion.LookRotation((target - shotStartPoint).normalized, Vector3.up);
        Rpc_ShootProjectile(shotStartPoint, rotation);
    }

    [ClientRpc]
    void Rpc_ShootProjectile(Vector3 shotStartPoint, Quaternion rotation)
    {
        if (shotPrefab)
        {
            ShotBase shotScript = Instantiate(shotPrefab, shotStartPoint, rotation);
            shotScript.Setup(shotStartPoint, rotation, this.gameObject);
        }
    }
}

