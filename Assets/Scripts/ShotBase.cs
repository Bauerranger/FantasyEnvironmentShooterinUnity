using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShotBase : MonoBehaviour
{
    public GameObject player;
    public void Setup(Vector3 initialPosition, Quaternion initialRotation, GameObject playerShooting)
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        player = playerShooting;
    }
}
