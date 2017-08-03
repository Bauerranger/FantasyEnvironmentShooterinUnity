using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightExplosion : MonoBehaviour {

    [SerializeField]
    private float addIntensity = 0.1f;
    


	void Update () {
        if (this.GetComponent<Light>().intensity >= 0.1f) { 
        this.GetComponent<Light>().intensity += addIntensity;
        if (this.GetComponent<Light>().intensity >= 10f)
        {
            addIntensity *= -1f;
        }
        }
    }
}
