using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossDead : IFSMState<EnemyController>
{

    private Animator animator;
    private bool dies = false;
    private NavMeshAgent agent;
    static readonly BossDead instance = new BossDead();
    public static BossDead Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        animator = e.GetComponent<Animator>();
        agent = e.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        e.GetComponent<Collider>().enabled = false;
    }

    public void Exit(EnemyController e)
    {
        e.GetComponent<NetworkEnemyManager>().ProxyCommandDie();
    }

    public void Reason(EnemyController e)
    {

    }

    public void Update(EnemyController e)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().bossIsDead = true;
        PlayDeathAnimation();
        if (e.dead)
            e.GetComponent<NetworkEnemyManager>().ProxyCommandDie();
    }

    void PlayDeathAnimation()
    {
        if (!dies)
            animator.SetTrigger("Die");
        dies = true;
    }


}
