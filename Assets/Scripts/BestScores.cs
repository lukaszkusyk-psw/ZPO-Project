using System.Collections.Generic;

[System.Serializable]
public class BestScores
{
    public List<ScoreData> scores = new List<ScoreData>();

    public BestScores()
    {
        scores = new List<ScoreData>();
    }
}
