using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
using static UIManager;

// This is a UI manager for the game scene. It controls the display of data.

// By OS

public class PlayMenuManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField]
    private GameObject playScreen;
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    public GameObject turnScreen;
    [SerializeField]
    private Image turnScreenImage;
    [SerializeField]
    private TextMeshProUGUI turnScreenText;
    [Space(5)]

    [SerializeField]
    private TextMeshProUGUI currentTurnTimeText;
    [SerializeField]
    private TextMeshProUGUI totalTurnTimeText;
    [SerializeField]
    private TextMeshProUGUI turnCountText;
    [SerializeField]
    private TextMeshProUGUI topBarTurnText;
    [Space(5)]

    [SerializeField]
    private TextMeshProUGUI endScreenText;
    [Space(10)]


    [Header("Team A")]
    [SerializeField]
    private Color teamAColour;
    [SerializeField]
    private string teamAName;
    [SerializeField]
    private TextMeshProUGUI teamACashText;
    [SerializeField]
    private TextMeshProUGUI teamAIncome;
    [SerializeField]
    private TextMeshProUGUI teamAPower;
    [SerializeField]
    private TextMeshProUGUI teamAResearch;
    [Space(5)]

    [Header("Team B")]
    [SerializeField]
    private Color teamBColour;
    [SerializeField]
    private string teamBName;
    [SerializeField]
    private TextMeshProUGUI teamBCashText;
    [SerializeField]
    private TextMeshProUGUI teamBIncome;
    [SerializeField]
    private TextMeshProUGUI teamBPower;
    [SerializeField]
    private TextMeshProUGUI teamBResearch;

    public static PlayMenuManager instance;
    // Set the UI to the correct format and data from the begining.
    private void Start()
    {
        instance = this;
        turnScreen.SetActive(true);
        endScreen.SetActive(false);
        playScreen.SetActive(true);
        SetUI();

        currentTurnTimeText.text = gm.currentTurnTime.ToString();

        var ts = TimeSpan.FromSeconds(gm.totalTurnTime);
        totalTurnTimeText.text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");

        teamACashText.text = $"$ {gm.teamACash}";
        teamBCashText.text = $"$ {gm.teamBCash}";
    }
     
    // Update UI that requires frequent updates
    private void Update()
    {
        UpdateUI();

        if (gm.currentTurnTime <= 0)
            OnEndTurnButtonClick();

        if (gm.gameEnded)
            EndGameUI(gm.teamALost);
    }

    // Check which team turn it is
    private bool IsBlueTurn()
    {
        if (gm.turnCount % 2 == 0)
            return true;
        else
            return false;
    }
    private void UpdateUI()
    {
        currentTurnTimeText.text = gm.currentTurnTime.ToString();

        var ts = TimeSpan.FromSeconds(gm.totalTurnTime);
        totalTurnTimeText.text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");

        teamACashText.text = $"$ {gm.teamACash}";
        teamBCashText.text = $"$ {gm.teamBCash}";

        teamAIncome.text = $"{gm.teamAIncome}";
        teamAPower.text = $"{gm.teamAPower}";
        teamAResearch.text = $"{gm.teamAResearch}";

        teamBIncome.text = $"{gm.teamBIncome}";
        teamBPower.text = $"{gm.teamBPower}";
        teamBResearch.text = $"{gm.teamBResearch}";
    }

    // Set the UI to the correct team turn
    public void SetUI()
    {
        if (IsBlueTurn())
        {
            turnScreenImage.color = teamAColour;
            turnScreenText.text = $"{teamAName} Turn";

            topBarTurnText.text = $"{teamAName} Turn:";
        }
        else
        {
            turnScreenImage.color = teamBColour;
            turnScreenText.text = $"{teamBName} Turn";

            topBarTurnText.text = $"{teamBName} Turn:";
        }

        turnCountText.text = $"Turn: {Mathf.CeilToInt(gm.turnCount / 2) + 1}";
    }

    private void EndGameUI(bool teamALost)
    {
        playScreen.SetActive(false);
        endScreen.SetActive(true);

        if (teamALost)
            endScreenText.text = $"! RED WINS !";
        else
            endScreenText.text = $"! BLUE WINS !";

        RectTransform endScreenRT = endScreen.GetComponent<RectTransform>();

        StartCoroutine(um.Lerp(endScreenRT, endScreenRT.anchoredPosition, new Vector2(0, 0), 0.5f));
    }

    //Buttons
    public void OnStartTurnButtonClick()
    {
        turnScreen.SetActive(false);
        playScreen.SetActive(true);

        gm.StartTurn();
    }
    
    public void OnEndTurnButtonClick()
    {
        if(gm.shootTurn != true)
        {
            gm.EndTurn();
            SetUI();
        }
    }

    public void OnReturnToMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
