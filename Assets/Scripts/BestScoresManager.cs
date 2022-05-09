using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BestScoresManager : MonoBehaviour
{
    public static BestScoresManager Instance;

    [TextArea(3, 10)]
    public string json;

    private BestScores bestScores;

    private void Awake()
    {
        Instance = this;
        LoadScores();
    }

    private void LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/BestScores.json") == false)
        {
            bestScores = new BestScores();
            return;
        }

        string jsonContent = File.ReadAllText(Application.persistentDataPath + "/BestScores.json");
        bestScores = JsonUtility.FromJson<BestScores>(jsonContent);
    }

    private void SaveScores()
    {
        string jsonContent = JsonUtility.ToJson(bestScores, true);
        File.WriteAllText(Application.persistentDataPath + "/BestScores.json", jsonContent);
        json = jsonContent;
    }

    public void AddScore(ScoreData scoreToAdd)
    {
        int scoresCount = bestScores.scores.Count;
        bool hasBeenSaved = false;

        for (int i = 0; i < scoresCount; i++)
        {
            if (scoreToAdd.playerDistance > bestScores.scores[i].playerDistance)
            {
                bestScores.scores.Insert(i, scoreToAdd);
                hasBeenSaved = true;
                break;
            }
        }

        if (scoresCount < 10 && hasBeenSaved == false)
            bestScores.scores.Add(scoreToAdd);

        SaveScores();

        PrepareLeaderboard();
    }

    public bool CanAddScore(float distance)
    {
        if (bestScores.scores.Count == 0)
            return true;

        if (distance > bestScores.scores[bestScores.scores.Count - 1].playerDistance)
            return true;

        return false;
    }

    public void PrepareLeaderboard()
    {
        UIManager.Instance.ShowLeaderbordScreen(bestScores.scores.Select(s => s.ToString()).ToArray());
    }

    private void FixScoresListLength()
    {
        for (int i = 10; i < bestScores.scores.Count; i++)
        {
            bestScores.scores.RemoveAt(10);
        }
    }

}
