using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastShot : NormalShot
{

    [SerializeField]
    private ShotBase[] shots;
    [SerializeField]
    private float waitTime = 0.01f;

    void Start()
    {
        StartCoroutine(WaitBetweenShots(waitTime));
    }

    IEnumerator WaitBetweenShots(float waitTime)
    {
        foreach (ShotBase shot in shots)
        {
            shotRigidbody = shot.GetComponent<Rigidbody>();
            shotRigidbody.AddForce(transform.forward * FireForce);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
