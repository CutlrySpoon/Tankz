using UnityEngine;

// This stores all core game data for other scripts to access

// By OS

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Check if instance exists already.
    public static GameManager gm
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("TileMapManager");
                instance = obj.AddComponent<GameManager>();
            }

            return instance;
        }
    }

    [Header("Game Settings")]
    [SerializeField]
    //Game Settings
    private int maxTurnTime = 60;
    [Space(5)]

    [SerializeField]
    private int startingCash = 500;
    [Space(5)]

    [SerializeField]
    public Color teamAColour;
    [SerializeField]
    public Color teamBColour;
    [Space(10)]

    [Header("Team Stats")]
    //Team
    public int teamACash = 0;
    public int teamAResearch = 0;
    public int teamAPower = 0;
    public int teamAIncome = 0;
    [Space(10)]

    public int teamBCash = 0;
    public int teamBResearch = 0;
    public int teamBPower = 0;
    public int teamBIncome = 0;

    //Game

    public int turnCount
    {
        get => TurnCount;
        private set
        {
            if (TurnCount == value) return;
            TurnCount = value;
            OnTurnChange?.Invoke();
        }
    }

    public bool inPlay { get; private set; } = false;
    public int currentTurnTime { get; private set; } = 0;
    public int totalTurnTime { get; private set; } = 0;

    public event System.Action OnTurnChange;
    private int TurnCount;


    //Logic
    float elapsedTime = 0;


    private void Awake()
    {
        instance = this;
    }

    // Reset timer
    private void Start()
    {
        currentTurnTime = maxTurnTime;

        teamACash = startingCash;
        teamBCash = startingCash;
    }

    private void Update()
    {
        Timer();
    }

    // Count up time
    private void Timer()
    {
        elapsedTime += Time.deltaTime;

        if (inPlay)
        {
            if (elapsedTime > 1f)
            {
                currentTurnTime--;
                totalTurnTime++;
                elapsedTime = 0;
            }
        }
    }

    public void StartTurn()
    {
        inPlay = true;

        if (turnCount % 2 == 0)
        {
            teamACash += teamAIncome;
        }
        else
        {
            teamBCash += teamBIncome;
        }
    }

    public void EndTurn()
    {
        inPlay = false;
        turnCount++;

        currentTurnTime = maxTurnTime;
    }
}
