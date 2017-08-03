﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWait : IFSMState<EnemyController>
{
    private NavMeshAgent agent;
    private List<GameObject> players = new List<GameObject>();
    static readonly EnemyPatrol instance = new EnemyPatrol();
    private int seeingDistance;
    public static EnemyPatrol Instance
    {
        get { return instance; }
    }

    public void Enter(EnemyController e)
    {
        seeingDistance = e.seeingDistance;
        agent = e.GetComponent<NavMeshAgent>();
    }

    public void Exit(EnemyController e)
    {
        Debug.Log("stopped waiting");
    }

    public void Reason(EnemyController e)
    {
        if (players.Count != GameObject.FindGameObjectsWithTag("Player").Length)
            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        RaycastHit hit;
        foreach (GameObject player in players)
        {
            if (Physics.Raycast(e.transform.position, (player.transform.position + new Vector3(0, 1, 0)) - e.transform.position, out hit, seeingDistance))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(e.transform.position, player.transform.position - e.transform.position, Color.red);
                    if (!e.usesRangedWeapons)
                    {
                        string state = ("EnemyChase");
                        e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, player);
                    }
                    if (e.usesRangedWeapons)
                    {
                        string state = ("EnemyAttack");
                        e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, player);
                    }
                }
                else
                {
                    Debug.DrawRay(e.transform.position, player.transform.position - e.transform.position, Color.green);
                }
            }

            if (e.Health <= 0)
            {
                string state = ("EnemyDead");
                e.GetComponent<NetworkEnemyManager>().ProxyCommandChangeState(state, players[0]);
            }
        }
    }

    public void Update(EnemyController e)
    {
    }
}
