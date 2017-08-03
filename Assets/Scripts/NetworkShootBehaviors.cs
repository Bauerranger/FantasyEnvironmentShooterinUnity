using System.Collections;
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
    private Transform ShootStartPoint;
    private Transform ShotTargetPoint;
    string player;
    public float durationOfPowerUp = 0;

    void Start()
    {
        if (!isLocalPlayer)
            return;
        EventManager.shotMethods += ProxyCommandNormalShot;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
        player = this.gameObject.GetComponent<NetworkMovement>().playerName;
    }

    private void Update()
    {
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
