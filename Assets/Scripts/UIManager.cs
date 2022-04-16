using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI distanceTMP;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        timeTMP.text = (PlayerController.Instance.roundTime - PlayerController.CurrentTime).ToString("00.000") + "s";
        distanceTMP.text = PlayerController.CurrentDistance.ToString("000.0");
    }
}
