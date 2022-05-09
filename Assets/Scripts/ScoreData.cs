using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData 
{
    public string playerName;
    public float playerDistance;

    public ScoreData(string playerName, float playerDistance)
    {
        this.playerName = playerName;
        this.playerDistance = playerDistance;
    }

    public override string ToString()
    {
        return (int)playerDistance + " - " + playerName;
    }
}
