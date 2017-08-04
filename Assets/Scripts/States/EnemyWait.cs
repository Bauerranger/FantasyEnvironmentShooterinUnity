using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWait : IFSMState<EnemyController>
{
    private NavMeshAgent agent;
    private List<GameObject> players = new List<GameObject>();
    static readonly EnemyPatrol instance = new EnemyPatrol();
    private int seeingDistance;
    public static EnemyPatrol Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        seeingDistance = e.seeingDistance;
        agent = e.GetComponent<NavMeshAgent>();
    }

    public void Exit(EnemyController e)
    {
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
    }
}
