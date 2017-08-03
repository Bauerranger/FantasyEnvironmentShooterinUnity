using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class AIController : NetworkBehaviour
{

    [SerializeField]
    private Transform destination;
    [SerializeField]
    private List<GameObject> deathParticles = new List<GameObject>();
    private NavMeshAgent agent;
    private Animator animator;
    private bool dead = false;

    public int health = 100;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (destination != null)
            agent.destination = destination.position;
    }

    void Update()
    {
        if (health <= 0)
        {
            agent.enabled = false;
            //playDeathAnimation();
        }
        if (!dead)
        {
            if (agent.velocity != Vector3.zero)
            {
                animator.SetBool("Run", true);
            }
            if (agent.velocity == Vector3.zero)
            {
                animator.SetBool("Run", false);
            }
        }
    }

    public void setDestination(Transform newDestination)
    {
        if (newDestination != null)
            agent.destination = newDestination.position;
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage)
    {
        //agent.velocity = Vector3.zero;
        animator.SetTrigger("Take Damage");
        health -= damage;
    }

    /*void playDeathAnimation()
    {
        if (!dead)
            animator.SetTrigger("Die");
        dead = true;
    }

    void selfDestruct()
    {
        if (!isServer)
            return;
        RpcSpawnDeath();
        RpcSelfDestruckt();
    }

    [ClientRpc]
    void RpcSelfDestruckt()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    [ClientRpc]
    void RpcSpawnDeath()
    {
        foreach (GameObject particle in deathParticles)
        {
            GameObject spawnedDeathParticles = Instantiate(particle, this.gameObject.transform.position, Quaternion.identity);
            NetworkServer.Spawn(spawnedDeathParticles);
        }
    }*/
}
