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

    /// <summary>
    /// If the player is killed, the enemy resets his behaviour
    /// </summary>
    /// <param name="e"></param>
    public void Exit(EnemyController e)
    {
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
            e.GetComponent<NetworkEnemyManager>().agent.stoppingDistance = 0;
        e.killedPlayer = false;
    }

    /// <summary>
    /// Enemys in waiting mode go back to the waiting stage (the wizards and the assasins do (the assasin has some weird bug where she begins to move on her own and i can´t find out why)
    /// When the player is killed or out of reach it should go back to patrol and (if the player is still in sight) go into chase again
    /// if the enemy has 0 health he dies
    /// </summary>
    /// <param name="e"></param>
    public void Reason(EnemyController e)
    {

        if (e.isWaiting && e.playersInReach.Count == 0)
        {
            string state = ("EnemyWait");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, attackedPlayer);
        }

        if (!e.isWaiting)
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
