using UnityEngine;

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

    //Game
    public bool inPlay { get; private set; } = false;
    public int turnCount { get; private set; } = 0;
    public int currentTurnTime { get; private set; } = 0;
    public int totalTurnTime { get; private set; } = 0;

    //Team
    public int teamACash { get; private set; } = 0;
    public int teamBCash { get; private set; } = 0;

    //Logic
    float elapsedTime = 0;


    private void Awake()
    {
        instance = this;
    }

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
    }

    public void EndTurn()
    {
        inPlay = false;
        turnCount++;

        currentTurnTime = maxTurnTime;
    }
}
