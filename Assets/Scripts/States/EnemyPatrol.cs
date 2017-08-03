using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : IFSMState<EnemyController>
{
    private NavMeshAgent agent;
    static readonly EnemyPatrol instance = new EnemyPatrol();
    private int seeingDistance;
    public static EnemyPatrol Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        e.isPatroling = true;
        agent = e.GetComponent<NavMeshAgent>();
        seeingDistance = e.seeingDistance;
        agent.stoppingDistance = 0;
        agent.destination = e.currentWaypoint.position;
    }

    public void Exit(EnemyController e)
    {
        e.isPatroling = false;
        Debug.Log("stopped patrolling");
    }

    public void Reason(EnemyController e)
    {
        if (e.playersInReach.Count > 0) //if for null did not work
        {
            string state = ("EnemyChase");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.playersInReach[0]);
        }
        
        if (e.Health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
        }
    }

    public void Update(EnemyController e)
    {
        if (agent.remainingDistance < 0.5f)
        {
            e.currentWaypoint.GetComponent<wayPointGiver>().giveWayPoint(e);
        }
        if (e.currentWaypoint != null && e.currentWaypoint.position != agent.destination)
            agent.destination = e.currentWaypoint.position;
    }
}
