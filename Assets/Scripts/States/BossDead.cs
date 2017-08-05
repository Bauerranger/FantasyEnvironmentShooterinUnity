using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossDead : IFSMState<EnemyController>
{

    private Animator animator;
    private bool dies = false;
    private NavMeshAgent agent;
    static readonly EnemyDead instance = new EnemyDead();
    public static EnemyDead Instance
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
        Debug.Log("stopped being Dead 'Doh'");
        agent.enabled = true;
        e.GetComponent<Collider>().enabled = true;
    }

    public void Reason(EnemyController e)
    {

    }

    public void Update(EnemyController e)
    {
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
