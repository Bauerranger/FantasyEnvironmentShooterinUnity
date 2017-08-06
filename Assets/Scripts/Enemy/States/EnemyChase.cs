using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : IFSMState<EnemyController>
{
    public EnemyChase(GameObject player)
    {
        chasedPlayer = player;
    }
    private GameObject chasedPlayer;
    public bool isInMaximumChasingDistance;
    public int maximumChasingDistance;
    private NavMeshAgent agent;

    public void Enter(EnemyController e)
    {
        maximumChasingDistance = e.maximumDistance;
        agent = e.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = e.maximumAttackDistance;
    }

    public void Exit(EnemyController e)
    {
    }

    public void Reason(EnemyController e)
    {
        agent.destination = chasedPlayer.transform.position;

        if (e.playersInReach.Count == 0)
        {
            string state = ("EnemyPatrol");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }

        if (agent.remainingDistance <= e.maximumAttackDistance)
        {
            string state = ("EnemyAttack");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }
        
        if (e.health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }
    }

    public void Update(EnemyController e)
    {
        e.UpdatePlayerDead();
    }
}
