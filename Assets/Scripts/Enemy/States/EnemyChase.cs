using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy state chasing the player
/// </summary>
public class EnemyChase : IFSMState<EnemyController>
{

    public EnemyChase(GameObject player)
    {
        chasedPlayer = player;
    }
    private GameObject chasedPlayer;
    public bool isInMaximumChasingDistance;
    public int maximumChasingDistance;

    /// <summary>
    /// Chechs for navmesh agent (client has none) and changes the max distance to the attacking distance.
    /// </summary>
    /// <param name="e"></param>
    public void Enter(EnemyController e)
    {
        maximumChasingDistance = e.maximumDistance;

        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
        {
            e.GetComponent<NetworkEnemyManager>().agent.stoppingDistance = e.maximumAttackDistance;
        }
        e.StartWaitForAttack();
    }


    /// <summary>
    /// sets waitBeforeAttack false because the enemy could otherwise go into attack immediately after the player comes into reach again
    /// </summary>
    /// <param name="e"></param>
    public void Exit(EnemyController e)
    {
        e.waitBeforeAttack = false;
    }

    /// <summary>
    /// Sets player as destination. Since player is always moving the destination has to be set every frame.
    /// If there are no players in reach or they are all dead, the enemy should go back to patrol mode.
    /// EnemyAttack should only be called when the player in attacking reach. Also the wait is for a bug, better described in the corrisponding function in EnemyController
    /// If the enemy has no life left he dies
    /// </summary>
    /// <param name="e"></param>
    public void Reason(EnemyController e)
    {
        if (e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled)
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeDestination(chasedPlayer);

        if (e.playersInReach.Count == 0)
        {
            string state = ("EnemyPatrol");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }

        if (e.waitBeforeAttack && e.GetComponent<NetworkEnemyManager>().agent.isActiveAndEnabled && e.GetComponent<NetworkEnemyManager>().agent.remainingDistance <= e.maximumAttackDistance)
        {
            string state = ("EnemyAttack");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }

        if (e.health <= 0)
        {
            string state = ("EnemyDead");
            e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, chasedPlayer);
        }
    }

    /// <summary>
    /// enemy is interested if player is dead so the e.playersInReach can be Updated
    /// </summary>
    /// <param name="e"></param>
    public void Update(EnemyController e)
    {
        e.UpdatePlayerDead();
    }
}
