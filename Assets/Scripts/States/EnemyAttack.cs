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
    private bool attacks = false;
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
    }

    public void Exit(EnemyController e)
    {
    }

    public void Reason(EnemyController e)
    {
        RaycastHit hit;
        if (Physics.Raycast(e.transform.position, attackedPlayer.transform.position - e.transform.position, out hit, e.maximumAttackDistance))
        {
            if (hit.transform.tag != "Player")
            {
                string state = ("EnemyPatrol");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
            }
        }
        if (e.Health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }
    }

    public void Update(EnemyController e)
    {
        if (!attacks)
        {
            e.GetComponent<EnemyAnimationManager>().playAttackAnimation();
            attacks = true;
        }
        else
        {
            e.GetComponent<EnemyAnimationManager>().attackAnimationEnds();
        }
    }

    public void AttackAnimationEnds()
    {
        attacks = false;
    }

    public void DoDamage()
    {
        attackedPlayer.GetComponent<NetworkMovement>().ReceiveDamage(enemyDamage);
    }
}
