using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointGiver : MonoBehaviour {

    [SerializeField]
    private Transform nextWayPoint;

    public void GiveWayPoint(EnemyController controller)
    {
        controller.currentWaypoint = nextWayPoint;
    }
}
