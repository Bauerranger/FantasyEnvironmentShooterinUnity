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

        if (e.health <= nextStageThreshold)
        {
            if (agent.remainingDistance >= e.maximumAttackDistance || e.playersInReach.Count == 0 || e.killedPlayer == true)
            {

                string state = ("BossStage2");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
            }
        }

        if (e.health <= 0)
        {
            string state = ("BossDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }
    }

    public void Update(EnemyController e)
    {
    }
}
