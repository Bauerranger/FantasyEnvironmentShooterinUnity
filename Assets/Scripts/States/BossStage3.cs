﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStage3 : IFSMState<EnemyController>
{
    private GameObject attackedPlayer;
    private NavMeshAgent agent;
    static readonly EnemyPatrol instance = new EnemyPatrol();
    public static EnemyPatrol Instance
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
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }
    }

    public void Update(EnemyController e)
    {
        agent.velocity = Vector3.zero;
        if (!e.attacks)
        {
            e.GetComponent<EnemyAnimationManager>().PlayAttackAnimation();
            e.attacks = true;
        }
        else
        {
            e.GetComponent<EnemyAnimationManager>().AttackAnimationEnds();
        }
    }
}

