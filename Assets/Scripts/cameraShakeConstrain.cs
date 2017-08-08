using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShakeConstrain : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam1noise;
    void Update()
    {
        if (!cam1noise.activeSelf)
        {
            cam1noise.transform.position = cam1.transform.position;
        }
    }
}
