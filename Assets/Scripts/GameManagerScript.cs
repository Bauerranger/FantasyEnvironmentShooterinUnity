using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public AudioClip Bossfight;
    public AudioClip Won;
    public AudioClip Lost;
    private List<GameObject> players = new List<GameObject>();
    public GameObject cam1;
    public GameObject cam1noise;
    public GameObject cam2;
    public GameObject cam2noise;
    [System.NonSerialized]
    public bool inBossfight = false;
    [System.NonSerialized]
    public bool bossIsDead = false;
    private bool bossCamActive = false;
    public Transform[] WinPosition;
    void Start()
    {
    }

    void Update()
    {
        if (bossIsDead && inBossfight)
        {
            StartCoroutine(WaitForBossDeath());
            inBossfight = false;
        }
        if (inBossfight)
        {
            if (!bossCamActive)
            {
                ActivateBossCam();
            }
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
    public void MakeScreenShake(float seconds)
    {
        StartCoroutine(ScreenShakeLength(seconds));
    }
    
    IEnumerator ScreenShakeLength(float seconds)
    {
        if (cam1.activeInHierarchy)
        {
            cam1noise.SetActive(true);
            cam1.SetActive(false);
            yield return new WaitForSeconds(seconds);
            cam1.SetActive(true);
            cam1noise.SetActive(false);
        }
        if (cam2.activeInHierarchy)
        {
            cam2noise.SetActive(true);
            cam2.SetActive(false);
            yield return new WaitForSeconds(seconds);
            cam2.SetActive(true);
            cam2noise.SetActive(false);
        }
    }

    private void ActivateBossCam()
    {
        cam1.SetActive(false);
        cam1noise.SetActive(false);
        cam2noise.SetActive(false);
        cam2.SetActive(true);
        bossCamActive = true;
    }
    
    IEnumerator WaitForBossDeath()
    {
        yield return new WaitForSeconds(2);
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

        cam1.SetActive(true);
        cam2.SetActive(false);
        cam1noise.SetActive(false);
        cam2noise.SetActive(false);
    }
}
