using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : IFSMState<EnemyController>
{

    public EnemyChase(GameObject player)
    {
        chasedPlayer = player;
    }
    private GameObject chasedPlayer;
    public bool isInMaximumChasingDistance;
    public int maximumChasingDistance;

    public void Enter(EnemyController e)
    {
        maximumChasingDistance = e.maximumDistance;

        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
        {
            e.GetComponent<NetworkEnemyManager>().agent.stoppingDistance = e.maximumAttackDistance;
        }
        e.StartWaitForAttack();
    }

    public void Exit(EnemyController e)
    {
        e.waitBeforeAttack = false;
    }

    public void Reason(EnemyController e)
    {
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeDestination(chasedPlayer);

        if (e.playersInReach.Count == 0)
        {
            string state = ("EnemyPatrol");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }

        if (e.waitBeforeAttack && e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled && e.GetComponent<NetworkEnemyManager>().agent.remainingDistance <= e.maximumAttackDistance)
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
