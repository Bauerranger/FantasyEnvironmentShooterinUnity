using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public AudioClip Bossfight;
    public AudioClip Won;
    public AudioClip Lost;
    private List<GameObject> players = new List<GameObject>();
    [System.NonSerialized]
    public bool inBossfight = false;
    [System.NonSerialized]
    public bool bossIsDead = false;
    public Transform[] WinPosition;
    void Start()
    {
    }

    void Update()
    {
        if (bossIsDead)
        {
            StartCoroutine(WaitForBossDeath());
        }
        if (inBossfight)
        {
            if (GetComponent<AudioSource>().clip != Bossfight)
            {
                GetComponent<AudioSource>().clip = Bossfight;
                AudioSource.PlayClipAtPoint(this.Bossfight, transform.position);
            }
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length != players.Count)
        {
            players.Clear();
            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        }
        if (players.Count > 0)
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<NetworkPlayerController>() != null && GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>() != null && player.GetComponent<NetworkPlayerController>().isDead)
                {
                    GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ActivateHighscore();
                    GetComponent<AudioSource>().clip = Lost;
                    AudioSource.PlayClipAtPoint(this.Lost, transform.position);
                }
            }
        }
    }

    IEnumerator WaitForBossDeath()
    {
        yield return new WaitForSecondsRealtime(5);
        if (GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>() != null)
            GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().ActivateHighscore();
        if (GetComponent<AudioSource>().clip != Won)
        {
            GetComponent<AudioSource>().clip = Won;
            AudioSource.PlayClipAtPoint(this.Won, transform.position);
        }
        int count = 0;
        foreach (GameObject player in players)
        {
            player.transform.position = WinPosition[count].position;
            player.transform.rotation = WinPosition[count].rotation;
            player.GetComponent<Animator>().SetBool("Talking", true);
            count++;
        }
    }
}
