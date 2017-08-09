using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Every shot needs to have a shotbase
/// </summary>
public class ShotBase : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject player;

    /// <summary>
    /// Sets the position, rotation and player who shot the projectile
    /// </summary>
    /// <param name="initialPosition"></param>
    /// <param name="initialRotation"></param>
    /// <param name="playerShooting"></param>
    public void Setup(Vector3 initialPosition, Quaternion initialRotation, GameObject playerShooting)
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        player = playerShooting;
    }
}
