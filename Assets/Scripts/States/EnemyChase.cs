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
        Debug.Log("started chasing");
    }

    public void Exit(EnemyController e)
    {
        Debug.Log("stopped chasing");
    }

    public void Reason(EnemyController e)
    {

        agent.destination = chasedPlayer.transform.position;

        if (e.playersInReach.Count == 0)
        {
            string state = ("EnemyPatrol");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }

        if (agent.remainingDistance <= e.maximumAttackDistance + 0.1f)
        {
            string state = ("EnemyAttack");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }


        if (e.Health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }
    }

    public void Update(EnemyController e)
    {

    }
}
