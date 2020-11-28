using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdated
{
    public ScoreUpdated(int score)
    {
        Score = score;
    }


    public int Score { get; }

}
