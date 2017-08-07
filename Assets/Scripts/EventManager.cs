using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public static event Action UpdateMethods;
    public static event Action buttonLeftMethods;
    public static event Action buttonRightMethods;
    public static event Action stopMethods;
    public static event Action buttonSpaceMethods;
    public static event Action attackMethods;
    public static event Action shotMethods;
    public static event Action hitEnemyMethods;
    public static event Action menuMethods;
    public static event Action playerStatusMethods;
    public static event Action dieMethods;
    public static event Action levitatesMethods;

    public void FixedUpdate()
    {
        if (UpdateMethods != null)
            UpdateMethods();
    }

    public static void buttonLeftIsPressed()
    {
        if (buttonLeftMethods != null)
            buttonLeftMethods();
    }

    public static void buttonRightIsPressed()
    {
        if (buttonRightMethods != null)
            buttonRightMethods();
    }

    public static void notLefnorRightIsPressed()
    {
        if (stopMethods != null)
            stopMethods();
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

    public static void levitates()
    {
        if (levitatesMethods != null)
            levitatesMethods();
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
        UpdateMethods = null;
        buttonLeftMethods = null;
        attackMethods = null;
        shotMethods = null;
        hitEnemyMethods = null;
        menuMethods = null;
        playerStatusMethods = null;
        dieMethods = null;
    }
}
