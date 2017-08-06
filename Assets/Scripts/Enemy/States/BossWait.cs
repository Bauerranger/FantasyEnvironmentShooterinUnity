using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossWait : IFSMState<EnemyController>
{
    private NavMeshAgent agent;
    private List<GameObject> players = new List<GameObject>();
    static readonly BossWait instance = new BossWait();
    public static BossWait Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        agent = e.GetComponent<NavMeshAgent>();
    }

    public void Exit(EnemyController e)
    {
    }

    public void Reason(EnemyController e)
    {
        if (e.isInBossfight == true)
        {
            string state = ("BossAttack");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
        }

        if (e.health <= 0)
        {
            string state = ("BossDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
        }
    }


    public void Update(EnemyController e)
    {
        agent.velocity = Vector3.zero;
    }
}
