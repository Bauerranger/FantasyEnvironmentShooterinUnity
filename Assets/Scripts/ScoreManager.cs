using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour
{

    public Dictionary<string, int> highscore;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        highscore = new Dictionary<string, int>();
        highscore.Add("Player 2", 0);
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
}
