﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;


enum shotType
{
    normal, mortar, through, rapid
}

[System.Serializable]
struct shotPair
{
    public shotType type;
    public ShotBase shotPrefab;
}

public class NetworkShootBehaviors : NetworkBehaviour
{

    [SerializeField]
    private shotPair[] shots;

    [Space(15)]

    [SerializeField]
    public Transform ShootStartPoint;
    private Transform ShotTargetPoint;
    private GameObject player;
    [System.NonSerialized]
    public float ammoOfPowerUp = 0;
    [System.NonSerialized]
    public int powerUpNo = 0;
    private int oldPowerUpNo = 0;

    void Start()
    {
        if (!isLocalPlayer)
            return;
        EventManager.shotMethods += ProxyCommandNormalShot;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
        player = this.gameObject;
    }

    private void Update()
    {
        if (oldPowerUpNo != powerUpNo)
        {
            switch (powerUpNo)
            {
                case 1:
                    EventManager.shotMethods -= ProxyCommandNormalShot;
                    EventManager.shotMethods += ProxyCommandThroughShot;
                    EventManager.shotMethods -= ProxyCommandMortarShot;
                    EventManager.shotMethods -= ProxyCommandRapidShot;
                    break;
                case 2:
                    EventManager.shotMethods -= ProxyCommandNormalShot;
                    EventManager.shotMethods -= ProxyCommandThroughShot;
                    EventManager.shotMethods += ProxyCommandMortarShot;
                    EventManager.shotMethods -= ProxyCommandRapidShot;
                    break;
                case 3:
                    EventManager.shotMethods -= ProxyCommandNormalShot;
                    EventManager.shotMethods -= ProxyCommandThroughShot;
                    EventManager.shotMethods -= ProxyCommandMortarShot;
                    EventManager.shotMethods += ProxyCommandRapidShot;
                    break;
                default:
                    EventManager.shotMethods += ProxyCommandNormalShot;
                    EventManager.shotMethods -= ProxyCommandThroughShot;
                    EventManager.shotMethods -= ProxyCommandMortarShot;
                    EventManager.shotMethods -= ProxyCommandRapidShot;
                    break;
            }
            oldPowerUpNo = powerUpNo;
        }
        if (player == null)
            player = this.gameObject;
        if (!isLocalPlayer)
            return;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
    }

    void ProxyCommandNormalShot()
    {
        CmdShoot(shotType.normal, ShotTargetPoint.position);
    }

    void ProxyCommandThroughShot()
    {
        CmdShoot(shotType.through, ShotTargetPoint.position);
    }

    void ProxyCommandMortarShot()
    {
        CmdShoot(shotType.mortar, ShotTargetPoint.position);
    }

    void ProxyCommandRapidShot()
    {
        CmdShoot(shotType.rapid, ShotTargetPoint.position);
    }

    [Command]
    void CmdShoot(shotType type, Vector3 targetWorldPosition)
    {
        Quaternion rotation = Quaternion.LookRotation((targetWorldPosition - ShootStartPoint.position).normalized, Vector3.up);
        Rpc_spawnShot(type, ShootStartPoint.position, rotation);
    }

    [ClientRpc]
    void Rpc_spawnShot(shotType type, Vector3 position, Quaternion rotation)
    {
        ShotBase shotPrefab = shots.Where(pair => pair.type == type).FirstOrDefault().shotPrefab;
        if (shotPrefab)
        {
            ShotBase shotScript = Instantiate(shotPrefab, position, rotation);
            shotScript.Setup(position, rotation, player);
        }
    }

    private void OnDestroy()
    {
        EventManager.shotMethods -= ProxyCommandNormalShot;
        EventManager.shotMethods -= ProxyCommandThroughShot;
        EventManager.shotMethods -= ProxyCommandMortarShot;
        EventManager.shotMethods -= ProxyCommandRapidShot;
    }

    private void OnDisable()
    {
        EventManager.shotMethods -= ProxyCommandNormalShot;
        EventManager.shotMethods -= ProxyCommandThroughShot;
        EventManager.shotMethods -= ProxyCommandMortarShot;
        EventManager.shotMethods -= ProxyCommandRapidShot;
    }
}
