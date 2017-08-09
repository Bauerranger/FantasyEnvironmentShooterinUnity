using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Give the collision to the FastShot script and let it handle that
/// </summary>

public class FastShotProjectile : MonoBehaviour {

    [SerializeField]
    GameObject SpawnedFrom;

    private void OnCollisionEnter(Collision collision)
    {
        SpawnedFrom.GetComponent<FastShot>().Collide(collision);
    }
}
