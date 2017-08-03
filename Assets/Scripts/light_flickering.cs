using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_flickering : MonoBehaviour {
    private float intensityChange;
    private bool sinks = true;
    private float randomThreshold;

    private void Start()
    {
        randomThreshold = Random.Range(0.75f, 0.9f);
    }
    void Update () {
        if (GetComponent<Light>().intensity >= 0.99f)
            sinks = true;

        if (GetComponent<Light>().intensity <= randomThreshold)
        {
            sinks = false;
            randomThreshold = Random.Range(0.6f, 0.9f);
        }

        if (sinks == true)
        intensityChange = Random.Range(-0.001f, -0.005f);
        else
        intensityChange = Random.Range(0.001f, 0.005f);

        GetComponent<Light>().intensity = GetComponent<Light>().intensity + intensityChange;
    }
}
