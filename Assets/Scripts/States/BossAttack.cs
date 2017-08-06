using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttack : IFSMState<EnemyController>
{
    private int nextStageThreshold = 1000;
    private GameObject attackedPlayer;
    private NavMeshAgent agent;
    static readonly BossAttack instance = new BossAttack();
    public static BossAttack Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        agent = e.GetComponent<NavMeshAgent>();
        agent.destination = e.currentWaypoint.position;
        e.GetComponent<EnemyAnimationManager>().BigBossJump();
    }

    public void Exit(EnemyController e)
    {
        agent.stoppingDistance = 0;
        e.killedPlayer = false;
    }

    public void Reason(EnemyController e)
    {

        if (e.health <= nextStageThreshold && !e.isInStage3)
        {
                string state = ("BossStage2");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }

        if (e.health <= 0)
        {
            string state = ("BossDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }

        if (e.isInStage3)
        {
            e.DelayChangeState("BossStage2", 6f);
        }
    }

    public void Update(EnemyController e)
    {
        if (agent.remainingDistance < 0.5f && e.currentWaypoint)
        {
            e.currentWaypoint.GetComponent<WayPointGiver>().GiveWayPoint(e);
        }
        if (e.currentWaypoint != null && e.currentWaypoint.position != agent.destination)
            agent.destination = e.currentWaypoint.position;
    }
}
