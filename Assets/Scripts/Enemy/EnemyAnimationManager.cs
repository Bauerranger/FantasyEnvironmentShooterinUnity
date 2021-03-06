﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationManager : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;
    [System.NonSerialized]
    public bool isAttacking = false;
    private bool isJumping = false;
    private bool reset = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!GetComponent<EnemyController>().isBoss)
        {
            if (!agent.velocity.Equals(Vector3.zero))
            {
                GetComponent<NetworkEnemyManager>().ProxyCommandSetRunBool(true);
            }
            else
            {
                GetComponent<NetworkEnemyManager>().ProxyCommandSetRunBool(false);
            }
        }
    }

    public void PlayAttackAnimation()
    {
        if (!isAttacking)
        {
            if (!GetComponent<EnemyController>().isWaiting)
            {
                float random = Random.Range(0, 3);
                if (random >= 0 && random < 1)
                    animator.SetTrigger("Melee Attack 01");
                if (random >= 1 && random < 2)
                    animator.SetTrigger("Melee Attack 02");
                if (random >= 2 && random < 4)
                    animator.SetTrigger("Melee Attack 03");
            }
            if (GetComponent<EnemyController>().isWaiting)
            {
                if (GetComponent<EnemyController>().isMage)
                {
                    float random = Random.Range(0, 3);
                    if (random >= 0 && random < 1)
                        animator.SetTrigger("Melee Attack");
                    if (random >= 1 && random < 2)
                        animator.SetTrigger("Cast Spell 01");
                    if (random >= 2 && random < 4)
                        animator.SetTrigger("Cast Spell 02");
                }
                else
                {
                    float random = Random.Range(0, 3);
                    if (random >= 0 && random < 1)
                        animator.SetTrigger("Melee Attack 01");
                    if (random >= 1 && random < 2)
                        animator.SetTrigger("Melee Attack 02");
                    if (random >= 2 && random < 4)
                        animator.SetTrigger("Throw Kunai");
                }
            }
            isAttacking = true;
        }
    }

    public void BigBossAttack()
    {
        if (!isAttacking && reset)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            reset = false;
            StartCoroutine(WaitForReset());
        }
    }

    public void BigBossJump()
    {
        if (!isJumping)
        {
            animator.SetBool("Hop", true);
            isJumping = true;
        }
    }

    public void AttackAnimationEnds()
    {
        isAttacking = false;
    }

    public void JumpAnimationEnds()
    {
        animator.SetBool("Hop", false);
        isJumping = false;
    }

    IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(2);
        reset = true;
    }

}
