using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Menu Screen")]
    public GameObject menuScreen;

    [Header("Player")]
    public GameObject playerScreen;
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

    [Header("Other")]
    public Button usefullButton;

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
        playerScreen.SetActiveOptimized(true);
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
        resultTMP.text = result.ToString("#####.0").Replace(',', '.') + "m";

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

    public void SaveResult()
    {
        BestScoresManager.Instance.AddScore(new ScoreData(nameInputFieldTMP.text, PlayerController.CurrentDistance));
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
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
        if (playerScreen.activeSelf)
        {
            timeTMP.text = (PlayerController.Instance.roundTime - PlayerController.CurrentTime).ToString("#0.000").Replace(',', '.') + "s";
            distanceTMP.text = "Distance: " + PlayerController.CurrentDistance.ToString("#####.0").Replace(',', '.') + "m";
        }
    }

    private void HideAllScreens()
    {
        menuScreen.SetActiveOptimized(false);
        playerScreen.SetActiveOptimized(false);
        gameOverScreen.SetActiveOptimized(false);
        resultScreen.SetActiveOptimized(false);
        leaderboardScreen.SetActiveOptimized(false);

        usefullButton.Select();
    }
}
