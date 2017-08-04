using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{

    public Dictionary<string, int> highscore;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        highscore = new Dictionary<string, int>();
    }

    public void ProxyCommandAddScore(string player, int score)
    {
        Cmd_AddScore(player, score);
    }

    [Command]
    void Cmd_AddScore(string player, int score)
    {
        if (isServer)
            Rpc_AddScore(player, score);
    }

    [ClientRpc]
    void Rpc_AddScore(string player, int score)
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length != players.Count)
        {
            players.Clear();
            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
            foreach (GameObject playerGameObject in players)
            {
                highscore.Add(playerGameObject.GetComponent<NetworkIdentity>().netId.ToString(), 0);
            }
        }
        highscore[player] += score;
        Debug.Log("New score for " + player + " is " + highscore[player]);
    }

    public void ActivateHighscore() //normally this would be in the GUIManager Script, but somehow I can not make any changes to the script anymore, since they do not compile somehow.
    {
        GameObject.FindGameObjectWithTag("highscore_Menu").GetComponent<Canvas>().enabled = true;
        if (GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().highscore.Count > 0)
        GameObject.FindGameObjectWithTag("scoreText1").GetComponentInChildren<Text>().text = (GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().highscore[players[0].GetComponent<NetworkIdentity>().netId.ToString()].ToString());
        if (GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().highscore.Count > 1)
            GameObject.FindGameObjectWithTag("scoreText2").GetComponentInChildren<Text>().text = (GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().highscore[players[1].GetComponent<NetworkIdentity>().netId.ToString()].ToString());
        Debug.Log("Git Good" + GameObject.FindGameObjectWithTag("HighscoreManager").GetComponent<ScoreManager>().highscore[players[0].GetComponent<NetworkIdentity>().netId.ToString()].ToString());
    }
}
