using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AddToHighscore : MonoBehaviour {
    [SerializeField]
    private int addScore = 3;
    [SerializeField]
    private GameObject effect;

    private void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ProxyCommandAddScore(other.GetComponent<NetworkIdentity>().netId.ToString(), addScore);
        GameObject spawnedParticle = Instantiate(effect, this.gameObject.transform.position, Quaternion.identity);
        spawnedParticle.GetComponent<SelfDestruct>().selfdestruct_in = 0.5f;
    }
}
