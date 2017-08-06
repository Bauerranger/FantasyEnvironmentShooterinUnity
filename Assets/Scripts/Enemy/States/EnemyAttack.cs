using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : IFSMState<EnemyController>
{
    public EnemyAttack(GameObject player)
    {
        attackedPlayer = player;
    }
    private GameObject attackedPlayer;
    static readonly EnemyPatrol instance = new EnemyPatrol();
    public static EnemyPatrol Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
    }

    public void Exit(EnemyController e)
    {
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
            e.GetComponent<NetworkEnemyManager>().agent.stoppingDistance = 0;
        e.killedPlayer = false;
    }

    public void Reason(EnemyController e)
    {


        if (e.usesRangedWeapons && e.playersInReach.Count == 0)
        {
            string state = ("EnemyWait");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }

        if (!e.usesRangedWeapons)
        {
            if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled && e.GetComponent<NetworkEnemyManager>().agent.remainingDistance >= e.maximumAttackDistance || e.playersInReach.Count == 0 || e.killedPlayer == true)
            {

                string state = ("EnemyPatrol");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
            }
        }

        if (e.health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }
    }

    public void Update(EnemyController e)
    {
        e.UpdatePlayerDead();
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
            e.GetComponent<NetworkEnemyManager>().agent.velocity = Vector3.zero;
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
