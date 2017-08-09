using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

//Names of the different shots
enum shotType
{
    normal, mortar, through, rapid
}

//Shotprefabs can be assigned their corrisponding names
[System.Serializable]
struct shotPair
{
    public shotType type;
    public ShotBase shotPrefab;
}

/// <summary>
/// Gives Player the abillity to shoot, processing which powerup is currently active
/// </summary>
public class NetworkShootBehaviors : NetworkBehaviour
{

    [SerializeField]
    private shotPair[] shots;

    [Space(15)]

    [SerializeField]
    public Transform ShootStartPoint;
    private Transform ShotTargetPoint;
    private GameObject player;
    public float ammoOfPowerUp = 0;
    public int powerUpNo = 0;
    private int oldPowerUpNo = 0; //helper to get to know which powerup should currently be active
    [SerializeField]
    private GameObject[] powerUpIndicator; // List of different effects that accure when a powerup is active
    private GameObject activePowerUpIndicator; // helperobject to determine which powerup indicator should be active

    /// <summary>
    /// Initiates the normal shot as standart shot, grabs the shot target and identifies the player (for highscore purposes)
    /// </summary>
    void Start()
    {
        if (!isLocalPlayer)
            return;
        EventManager.shotMethods += ProxyCommandNormalShot;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
        player = this.gameObject;
    }

    /// <summary>
    /// Determines how much ammonition is left if a powerup is active, determines which powerup is active, postitions the powerup indicator at the playerposition
    /// </summary>
    private void Update()
    {
        if (ammoOfPowerUp <= 0)
        {
            powerUpNo = 0;
        }
        if (oldPowerUpNo != powerUpNo)
        {
            switch (powerUpNo)
            {
                case 0:
                    EventManager.shotMethods += ProxyCommandNormalShot;
                    EventManager.shotMethods -= ProxyCommandMortarShot;
                    EventManager.shotMethods -= ProxyCommandThroughShot;
                    EventManager.shotMethods -= ProxyCommandRapidShot;
                    if (activePowerUpIndicator != null)
                    {
                        Destroy(activePowerUpIndicator);
                    }
                    break;
                case 1:
                    EventManager.shotMethods -= ProxyCommandNormalShot;
                    EventManager.shotMethods += ProxyCommandMortarShot;
                    EventManager.shotMethods -= ProxyCommandThroughShot;
                    EventManager.shotMethods -= ProxyCommandRapidShot;
                    if (activePowerUpIndicator != null)
                    {
                        Destroy(activePowerUpIndicator);
                    }
                    activePowerUpIndicator = Instantiate(powerUpIndicator[0], this.transform.position, this.transform.rotation);
                    break;
                case 2:
                    EventManager.shotMethods -= ProxyCommandNormalShot;
                    EventManager.shotMethods -= ProxyCommandMortarShot;
                    EventManager.shotMethods += ProxyCommandThroughShot;
                    EventManager.shotMethods -= ProxyCommandRapidShot;
                    if (activePowerUpIndicator != null)
                    {
                        Destroy(activePowerUpIndicator);
                    }
                    activePowerUpIndicator = Instantiate(powerUpIndicator[1], this.transform.position, this.transform.rotation);
                    break;
                case 3:
                    EventManager.shotMethods -= ProxyCommandNormalShot;
                    EventManager.shotMethods -= ProxyCommandMortarShot;
                    EventManager.shotMethods -= ProxyCommandThroughShot;
                    EventManager.shotMethods += ProxyCommandRapidShot;
                    if (activePowerUpIndicator != null)
                    {
                        Destroy(activePowerUpIndicator);
                    }
                    activePowerUpIndicator = Instantiate(powerUpIndicator[2], this.transform.position, this.transform.rotation);

                    break;
                default:
                    break;
            }
            oldPowerUpNo = powerUpNo;
        }
        if (player == null)
            player = this.gameObject;
        if (!isLocalPlayer)
            return;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
        if (activePowerUpIndicator != null)
        {
            activePowerUpIndicator.transform.position = this.transform.position + new Vector3(0, 0.1f, 0); //plus new Vector3 because it otherwise would be under the floor
        }
    }

    /// <summary>
    /// Gives shotinformation to the Network
    /// </summary>
    void ProxyCommandNormalShot()
    {
        CmdShoot(shotType.normal, ShotTargetPoint.position);
    }

    /// <summary>
    /// Gives shotinformation to the Network
    /// </summary>
    void ProxyCommandThroughShot()
    {
        CmdShoot(shotType.through, ShotTargetPoint.position);
    }

    /// <summary>
    /// Gives shotinformation to the Network
    /// </summary>
    void ProxyCommandMortarShot()
    {
        CmdShoot(shotType.mortar, ShotTargetPoint.position);
    }

    /// <summary>
    /// Gives shotinformation to the Network
    /// </summary>
    void ProxyCommandRapidShot()
    {
        CmdShoot(shotType.rapid, ShotTargetPoint.position);
    }

    /// <summary>
    /// Sets initial shotdirection and counts down ammunition for the shots
    /// </summary>
    [Command]
    void CmdShoot(shotType type, Vector3 targetWorldPosition)
    {
        if (!isServer)
        {
            return;
        }
        if (ammoOfPowerUp > 0)
            ammoOfPowerUp--;
        Quaternion rotation = Quaternion.LookRotation((targetWorldPosition - ShootStartPoint.position).normalized, Vector3.up);
        Rpc_spawnShot(type, ShootStartPoint.position, rotation);
    }

    /// <summary>
    /// Spawns shots
    /// </summary>
    /// <param name="type">Type of shot that will be spawned</param>
    /// <param name="position">Position where the shot will be spawned</param>
    /// <param name="rotation">Direction the shot will fly to</param>
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
    /// <summary>
    /// When this script is destroyed, all methods should be unsubscribed
    /// </summary>
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
