using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPointGiver : MonoBehaviour {

    [SerializeField]
    private Transform nextWayPoint;

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && nextWayPoint != null && other.GetComponent<EnemyController>().isPatroling)
        {
            other.GetComponent<EnemyController>().currentWaypoint = nextWayPoint;
        }
    }*/

    public void giveWayPoint(EnemyController controller)
    {
        controller.currentWaypoint = nextWayPoint;
    }
}
