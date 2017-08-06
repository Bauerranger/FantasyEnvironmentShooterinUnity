using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : IFSMState<EnemyController>
{
    static readonly EnemyPatrol instance = new EnemyPatrol();
    private int seeingDistance;
    public static EnemyPatrol Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        e.isPatroling = true;
        seeingDistance = e.seeingDistance;
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
        e.GetComponent<NetworkEnemyManager>().agent.stoppingDistance = 0;
        if (e.currentWaypoint)
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeDestination(e.currentWaypoint.gameObject);
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
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled && e.GetComponent<NetworkEnemyManager>().agent.remainingDistance < 0.5f && e.currentWaypoint)
        {
            e.currentWaypoint.GetComponent<WayPointGiver>().GiveWayPoint(e);
        }
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled && e.currentWaypoint != null && e.currentWaypoint.position != e.GetComponent<NetworkEnemyManager>().agent.destination)
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeDestination(e.currentWaypoint.gameObject);
    }
}
