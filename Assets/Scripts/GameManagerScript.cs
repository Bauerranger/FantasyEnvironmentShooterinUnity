using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public AudioClip Bossfight;
    public AudioClip Won;
    public AudioClip Lost;
    private List<GameObject> players = new List<GameObject>();
    public bool inBossfight;
    public bool bossIsDead;
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
                if (player.GetComponent<NetworkPlayerController>() != null && !player.GetComponent<NetworkPlayerController>().isDead)
                    return;
                if (player.GetComponent<NetworkPlayerController>() != null && player.GetComponent<NetworkPlayerController>().isDead && GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>() != null)
                    GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ActivateHighscore();
            }
            GetComponent<AudioSource>().clip = Lost;
            GetComponent<AudioSource>().Play();
        }
        if (bossIsDead)
        {
            if (GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>() != null)
                GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ActivateHighscore();
            foreach (GameObject player in players)
            {

            }
            GetComponent<AudioSource>().clip = Won;
            GetComponent<AudioSource>().Play();
        }

        if (inBossfight)
        {
            GetComponent<AudioSource>().clip = Bossfight;
            GetComponent<AudioSource>().Play();
        }
    }
}
