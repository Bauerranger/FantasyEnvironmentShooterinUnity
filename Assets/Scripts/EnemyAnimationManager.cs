using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationManager : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!agent.velocity.Equals(Vector3.zero))
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public void PlayAttackAnimation()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("Melee Attack 01");
            isAttacking = true;
        }
    }

    public void AttackAnimationEnds()
    {
        isAttacking = false;
    }
}
