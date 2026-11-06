using System.Collections;
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

    //Game
    public bool inPlay { get; private set; } = false;
    public int turnCount { get; private set; } = 0;
    public int currentTurnTime { get; private set; } = 0;
    public int totalTurnTime { get; private set; } = 0;
    public bool shootTurn { get; private set; } = false;

    //Team
    public int teamACash { get; private set; } = 0;
    public int teamBCash { get; private set; } = 0;

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
    }

    public void EndTurn()
    {
        inPlay = false;
        ShootTurn();
        turnCount++;

        currentTurnTime = maxTurnTime;
    }

    IEnumerator ShootTurn()
    {
        shootTurn = true;
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        if (bullets.Length > 0)
        {
            WaitUntil wait = new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bullet").Length == 0);
            yield return wait;
        }
        shootTurn = false;
        yield return new WaitForSeconds(1f);
    }
}
