using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastShotProjectile : MonoBehaviour {

    [SerializeField]
    GameObject SpawnedFrom;

    private void OnCollisionEnter(Collision collision)
    {
        SpawnedFrom.GetComponent<FastShot>().Collide(collision);
    }
}
