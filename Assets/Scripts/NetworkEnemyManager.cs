using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkEnemyManager : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> deathParticles = new List<GameObject>();


    public void ProxyCommandTakeDamage(int damageTaken, string player)
    {
        Cmd_TakeDamage(damageTaken, player);
    }

    [Command]
    void Cmd_TakeDamage(int damageTaken, string player)
    {
        Rpc_TakeDamage(damageTaken, player);
    }

    [ClientRpc]
    void Rpc_TakeDamage(int damageTaken, string player)
    {
        this.GetComponent<EnemyController>().Health -= damageTaken;
        GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().addScore(player, damageTaken);
        if (this.GetComponent<EnemyController>().Health <= 0)
            GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().addScore(player, this.GetComponent<EnemyController>().deathScore);
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
            NetworkServer.Spawn(spawnedDeathParticles);
        }
        NetworkServer.Destroy(this.gameObject);
    }

}

