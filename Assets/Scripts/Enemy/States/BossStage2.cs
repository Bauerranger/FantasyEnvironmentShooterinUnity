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

        if (e.health <= nextStageThreshold && !e.isInStage3)
        {
                e.isInStage3 = true;
        }

        if (e.health <= 0)
        {
            string state = ("BossDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, e.gameObject);
        }

        if (e.isInStage3)
        {
            e.DelayChangeState("BossAttack", 4f);
        }
    }

    public void Update(EnemyController e)
    {
            e.GetComponent<EnemyAnimationManager>().BigBossAttack();
    }

}
