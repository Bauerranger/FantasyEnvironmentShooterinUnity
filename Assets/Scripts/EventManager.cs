using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action movementMethods;
    public static event Action buttonAMethods;
    public static event Action buttonDMethods;
    public static event Action buttonSpaceMethods;
    public static event Action attackMethods;
    public static event Action shotMethods;
    public static event Action hitEnemyMethods;
    public static event Action menuMethods;
    public static event Action playerStatusMethods;
    public static event Action dieMethods;

    public static void Movement()
    {
        if (movementMethods != null)
            movementMethods();
    }

    public static void buttonAIsPressed()
    {
        if (buttonAMethods != null)
            buttonAMethods();
    }

    public static void buttonDIsPressed()
    {
        if (buttonDMethods != null)
            buttonDMethods();
    }

    public static void buttonSpaceIsPressed()
    {
        if (buttonSpaceMethods != null)
            buttonSpaceMethods();
    }

    public static void Attack()
    {
        if (attackMethods != null)
            attackMethods();
    }

    public static void Shoot()
    {
        if (shotMethods != null)
        {
            shotMethods();
        }
    }

    public static void HitEnemy()
    {
        if (shotMethods != null)
        {
            hitEnemyMethods();
        }
    }

    public static void Menu()
    {
        if (menuMethods != null)
        {
            menuMethods();
        }
    }

    public static void ObservePlayerStatus()
    {
        if (playerStatusMethods != null)
        {
            playerStatusMethods();
        }
    }

    public static void Die()
    {
        if (dieMethods != null)
        {
            dieMethods();
        }
    }

    private void OnDestroy()
    {
        movementMethods = null;
        buttonAMethods = null;
        attackMethods = null;
        shotMethods = null;
        hitEnemyMethods = null;
        menuMethods = null;
        playerStatusMethods = null;
        dieMethods = null;
    }
}
