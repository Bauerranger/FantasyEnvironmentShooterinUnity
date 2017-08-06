using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStage3 : IFSMState<EnemyController>
{
    private bool isJumping = false;
    private NavMeshAgent agent;
    static readonly BossStage3 instance = new BossStage3();
    public static BossStage3 Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        agent = e.GetComponent<NavMeshAgent>();
    }

    public void Exit(EnemyController e)
    {
        agent.stoppingDistance = 0;
        e.killedPlayer = false;
    }

    public void Reason(EnemyController e)
    {
        if (e.health <= 0)
        {
            string state = ("BossDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
        }
    }

    public void Update(EnemyController e)
    {
        if (isJumping)
        {
            e.GetComponent<EnemyAnimationManager>().BigBossJump();
        }
        if (agent.remainingDistance < 0.5f && e.currentWaypoint)
        {
            e.currentWaypoint.GetComponent<WayPointGiver>().GiveWayPoint(e);
        }
        if (e.currentWaypoint != null && e.currentWaypoint.position != agent.destination)
            agent.destination = e.currentWaypoint.position;
    }
}

