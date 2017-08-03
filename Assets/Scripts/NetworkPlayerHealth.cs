using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerHealth : NetworkBehaviour {

    public int health;
    private int oldHealth;

    private void Start()
    {
        EventManager.playerStatusMethods += ObserveHealth;
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
    }

    public void ObserveHealth()
    {
        if (health <= 0)
            GetComponent<NetworkPlayerController>().dies = true;
    }

    private void Update()
    {
        if (health < oldHealth)
        {
            Cmd_SetHealth(health);
            oldHealth = health;
        }
        Debug.Log("I am  " + GetComponent<NetworkPlayerController>().playerName + " and my health is " + health);
    }

    [Command]
    void Cmd_SetHealth(int newHealth)
    {
        Rpc_SetHealth(newHealth);
    }

    [ClientRpc]
    void Rpc_SetHealth(int newHealth)
    {
        health = newHealth;
    }
}
