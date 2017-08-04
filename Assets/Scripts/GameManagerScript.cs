using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length != players.Count)
        {
            players.Clear();
            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        }
        if (players.Count > 0)
        {
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<NetworkPlayerController>().isDead)
                    return;
                if (player.GetComponent<NetworkPlayerController>().isDead)
                    GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ActivateHighscore();
            }
        }
    }
}
