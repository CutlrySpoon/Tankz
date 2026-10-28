using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using static GameManager;
using static UIManager;

public class PlayMenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject turnScreen;
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
    [Space(5)]

    [SerializeField]
    private TextMeshProUGUI topBarTurnText;
    [Space(10)]


    [Header("Team A")]
    [SerializeField]
    private Color teamAColour;
    [SerializeField]
    private string teamAName;
    [SerializeField]
    private TextMeshProUGUI teamACashText;
    [Space(5)]

    [Header("Team B")]
    [SerializeField]
    private Color teamBColour;
    [SerializeField]
    private string teamBName;
    [SerializeField]
    private TextMeshProUGUI teamBCashText;


    private void Start()
    {
        SetUI();

        currentTurnTimeText.text = gm.currentTurnTime.ToString();

        var ts = TimeSpan.FromSeconds(gm.totalTurnTime);
        totalTurnTimeText.text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");

        teamACashText.text = $"$ {gm.teamACash}";
        teamBCashText.text = $"$ {gm.teamBCash}";
    }

    private void Update()
    {
        UpdateUI();

        if (gm.currentTurnTime <= 0)
            OnEndTurnButtonClick();
    }

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
    }

    private void SetUI()
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

    //Buttons
    public void OnStartTurnButtonClick()
    {
        turnScreen.SetActive(false);
        gm.StartTurn();
    }
    
    public void OnEndTurnButtonClick()
    {
        gm.EndTurn();
        SetUI();
        turnScreen.SetActive(true);
    }
}
