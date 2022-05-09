using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Menu Screen")]
    public GameObject menuScreen;

    [Header("Stats")]
    public GameObject statsScreen;
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI distanceTMP;

    [Header("Game Over Screen")]
    public GameObject gameOverScreen;

    [Header("Result Screen")]
    public GameObject resultScreen;
    public TextMeshProUGUI resultTMP;
    public TMP_InputField nameInputFieldTMP;

    [Header("Leaderboard Screen")]
    public GameObject leaderboardScreen;
    public TextMeshProUGUI[] leaderLinesTMP;

    public void ShowMenu()
    {
        HideAllScreens();
        menuScreen.SetActiveOptimized(true);
        AudioManager.instance.UpdateMusic(true);
        PlayerController.Instance.DestroyOldObjects();
    }

    public void ShowGameUI()
    {
        HideAllScreens();
        statsScreen.SetActiveOptimized(true);
    }

    public void ShowGameOverScreen()
    {
        HideAllScreens();
        gameOverScreen.SetActiveOptimized(true);
    }

    public void ShowResultScreen(float result, bool showInputField)
    {
        HideAllScreens();
        resultScreen.SetActiveOptimized(true);
        resultTMP.text = result.ToString("000.0");

        if (showInputField == false)
        {
            nameInputFieldTMP.gameObject.SetActiveOptimized(false);
            return;
        }

        nameInputFieldTMP.gameObject.SetActiveOptimized(true);
        nameInputFieldTMP.text = string.Empty;
    }

    public void ShowLeaderbordScreen(string[] bestScores)
    {
        HideAllScreens();
        leaderboardScreen.SetActiveOptimized(true);

        int scoresCount = bestScores.Length;

        if (scoresCount > leaderLinesTMP.Length)
            scoresCount = leaderLinesTMP.Length;

        for (int i = 0; i < scoresCount; i++)
        {
            leaderLinesTMP[i].text = bestScores[i];
        }

        for (int i = scoresCount; i < leaderLinesTMP.Length; i++)
        {
            leaderLinesTMP[i].text = "-";
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ShowMenu();
    }

    private void Update()
    {
        if (statsScreen.activeSelf)
        {
            timeTMP.text = (PlayerController.Instance.roundTime - PlayerController.CurrentTime).ToString("00.000") + "s";
            distanceTMP.text = PlayerController.CurrentDistance.ToString("000.0");
        }
    }

    private void HideAllScreens()
    {
        menuScreen.SetActiveOptimized(false);
        statsScreen.SetActiveOptimized(false);
        gameOverScreen.SetActiveOptimized(false);
        resultScreen.SetActiveOptimized(false);
        leaderboardScreen.SetActiveOptimized(false);
    }
}
