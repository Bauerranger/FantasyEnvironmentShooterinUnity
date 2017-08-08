using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour {

    public GameObject toDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach(GameObject enemy in toDestroy.GetComponent<BattleManager>().enemysAlive)
            {
                Destroy(enemy);
            }
            Destroy(toDestroy);
        }
    }
}
