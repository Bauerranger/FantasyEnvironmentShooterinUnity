using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public Dictionary<string, int> highscore;

    void Start()
    {
        highscore = new Dictionary<string, int>();
        highscore.Add("Player 1", 0);
        highscore.Add("Player 2", 0);
    }

    public void addScore(string player, int score)
    {
        highscore[player] += score;
    }
}
