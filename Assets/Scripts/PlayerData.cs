using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<int> scores = new();

    public PlayerData(List<int> prevScores, float score)
    {
        this.scores = prevScores;
        this.scores.Add((int)score);
    }

    public void SortScore()
    {
        if(scores.Count > 5) {
            if(scores[4] < scores[5]) {
                scores[4] = scores[5];
                scores.RemoveAt(4);
            } else scores.RemoveAt(5);
        }

        scores.Sort();
        scores.Reverse();
    }
}
