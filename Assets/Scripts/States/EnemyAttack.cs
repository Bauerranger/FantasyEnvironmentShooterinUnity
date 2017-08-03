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
    private int enemyDamage;
    private Transform destination;
    private NavMeshAgent agent;
    static readonly EnemyPatrol instance = new EnemyPatrol();
    public static EnemyPatrol Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        enemyDamage = e.enemyDamage;
        agent = e.GetComponent<NavMeshAgent>();
        Debug.Log("started attack");
    }

    public void Exit(EnemyController e)
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(attackedPlayer.transform.position);
        Debug.Log("stopped attack");
    }

    public void Reason(EnemyController e)
    {
        agent.destination = attackedPlayer.transform.position;

        if (agent.remainingDistance >= e.maximumAttackDistance || e.playersInReach[0] == null || e.killedPlayer == true)
        {
            string state = ("EnemyChase");
            e.killedPlayer = false;
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
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



    public void DoDamage()
    {
        attackedPlayer.GetComponent<NetworkPlayerHealth>().ReceiveDamage(enemyDamage);
    }
}
