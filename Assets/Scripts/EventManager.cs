using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public delegate void movementDelegate();
    public static event movementDelegate movementMethods;
    public delegate void attackDelegate();
    public static event attackDelegate attackMethods;
    public delegate void shotDelegate();
    public static event shotDelegate shotMethods;
    public delegate void hitEnemyDelegate();
    public static event hitEnemyDelegate hitEnemyMethods;
    public delegate void menuDelegate();
    public static event menuDelegate menuMethods;

    public static void movement()
    {
        if (movementMethods != null)
            movementMethods();
    }

    public static void attack()
    {
        if (attackMethods != null)
            attackMethods();
    }

    public static void shoot()
    {
        if (shotMethods != null)
        {
            shotMethods();
        }
    }

    public static void hitEnemy()
    {
        if (shotMethods != null)
        {
            hitEnemyMethods();
        }
    }

    public static void menu()
    {
        if (menuMethods != null)
        {
            menuMethods();
        }
    }
}
