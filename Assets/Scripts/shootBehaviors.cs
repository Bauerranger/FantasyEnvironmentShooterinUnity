using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class shootBehaviors : NetworkBehaviour
{

    [SerializeField]
    private ShotBase[] shots;
    [SerializeField]
    private Transform ShootStartPoint;

    public float durationOfPowerUp = 0;

    void Start()
    {
            EventManager.shotMethods += normalShot;
        
    }

    void normalShot()
    {
            EventManager.shotMethods -= throughShot;
            EventManager.shotMethods -= mortarShot;
            EventManager.shotMethods -= rapidShot;

            ShotBase spawnedShot = Instantiate(shots[0], ShootStartPoint.position, Quaternion.identity);
            if (spawnedShot)
            {
                spawnedShot.Setup(ShootStartPoint.position, Quaternion.LookRotation((transform.position - ShootStartPoint.position).normalized, Vector3.up), this.gameObject.name);
            }
        
    }

    void throughShot()
    {
        EventManager.shotMethods -= normalShot;
        EventManager.shotMethods -= mortarShot;
        EventManager.shotMethods -= rapidShot;

        Instantiate(shots[1], ShootStartPoint.position, Quaternion.identity);
    }

    void mortarShot()
    {
        EventManager.shotMethods -= normalShot;
        EventManager.shotMethods -= throughShot;
        EventManager.shotMethods -= rapidShot;

        Instantiate(shots[2], ShootStartPoint.position, Quaternion.identity);
    }

    void rapidShot()
    {
        EventManager.shotMethods -= normalShot;
        EventManager.shotMethods -= throughShot;
        EventManager.shotMethods -= mortarShot;

        Instantiate(shots[3], ShootStartPoint.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        EventManager.shotMethods -= normalShot;
        EventManager.shotMethods -= throughShot;
        EventManager.shotMethods -= mortarShot;
        EventManager.shotMethods -= rapidShot;
    }

    private void OnDisable()
    {
        EventManager.shotMethods -= normalShot;
        EventManager.shotMethods -= throughShot;
        EventManager.shotMethods -= mortarShot;
        EventManager.shotMethods -= rapidShot;
    }
}
