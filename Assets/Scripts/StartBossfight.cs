using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossfight : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().inBossfight = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
