using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStage2 : IFSMState<EnemyController>
{
    private int nextStageThreshold = 600;
    private NavMeshAgent agent;
    static readonly BossStage2 instance = new BossStage2();
    public static BossStage2 Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        agent = e.GetComponent<NavMeshAgent>();
        e.GetComponent<EnemyAnimationManager>().JumpAnimationEnds();
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

                string state = ("BossStage3");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
            }
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
        if (!e.attacks)
        {
            e.GetComponent<EnemyAnimationManager>().BigBossAttack();
            e.attacks = true;
        }
        else
        {
            e.GetComponent<EnemyAnimationManager>().AttackAnimationEnds();
        }
    }
}
