using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This script handles the playerhealth. It had to be in a seperate script, because the NetworkPlayerController had been destroyed previously.
/// </summary>
public class NetworkPlayerHealth : NetworkBehaviour
{

    public int health;
    private int oldHealth;

    /// <summary>
    /// Playerhealth subscribes to the Eventmanager
    /// </summary>
    private void Start()
    {
        EventManager.playerStatusMethods += ObserveHealth;
    }

    /// <summary>
    /// Takes the ammount of damage from the current health
    /// </summary>
    /// <param name="damage">Damage dealt by the enemy</param>
    public void ReceiveDamage(int damage)
    {
        health -= damage;
    }

    /// <summary>
    /// Sees if player has 0 health. If so, player dies.
    /// </summary>
    public void ObserveHealth()
    {
        if (health < oldHealth)
        {
            Cmd_SetHealth(health);
            oldHealth = health;
        }
        if (health <= 0)
        {
            EventManager.playerStatusMethods -= ObserveHealth;
            GetComponent<NetworkPlayerController>().dies = true;
        }
    }

    [Command]
    void Cmd_SetHealth(int newHealth)
    {
        Rpc_SetHealth(newHealth);
    }

    /// <summary>
    /// Sets health to newHealth so the playerhealth is synchronised throug the network
    /// </summary>
    /// <param name="newHealth"></param>
    [ClientRpc]
    void Rpc_SetHealth(int newHealth)
    {
        health = newHealth;
    }

    private void OnDestroy()
    {
        EventManager.playerStatusMethods -= ObserveHealth;
    }
}
