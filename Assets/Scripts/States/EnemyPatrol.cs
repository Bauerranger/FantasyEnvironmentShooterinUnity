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
        if (e.currentWaypoint)
        agent.destination = e.currentWaypoint.position;
    }

    public void Exit(EnemyController e)
    {
        e.isPatroling = false;
    }

    public void Reason(EnemyController e)
    {
        if (e.playersInReach.Count > 0)
        {
            string state = ("EnemyChase");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.playersInReach[0]);
        }
        
        if (e.health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
        }
    }

    public void Update(EnemyController e)
    {
        e.UpdatePlayerDead();
        if (agent.remainingDistance < 0.5f && e.currentWaypoint)
        {
            e.currentWaypoint.GetComponent<WayPointGiver>().GiveWayPoint(e);
        }
        if (e.currentWaypoint != null && e.currentWaypoint.position != agent.destination)
            agent.destination = e.currentWaypoint.position;
    }
}
