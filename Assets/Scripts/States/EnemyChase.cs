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
    }

    public void Exit(EnemyController e)
    {
        Debug.Log("stopped chasing");
    }

    public void Reason(EnemyController e)
    {

        agent.destination = chasedPlayer.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(e.transform.position, chasedPlayer.transform.position - e.transform.position, out hit, maximumChasingDistance))
        {
            if (hit.transform.tag != "Player")
            {
                string state = ("EnemyPatrol");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
            }
        }
        if (Physics.Raycast(e.transform.position, chasedPlayer.transform.position - e.transform.position, out hit, e.maximumAttackDistance))
        {
            if (hit.transform.tag == "Player")
            {
                string state = ("EnemyAttack");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
            }
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
