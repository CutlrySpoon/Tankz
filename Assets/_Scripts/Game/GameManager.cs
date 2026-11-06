using System.Collections;
using Unity.VisualScripting;
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
    public bool shootTurn = false;

    //Team
    public int teamACash { get; private set; } = 0;
    public int teamBCash { get; private set; } = 0;

    public bool blueTurn;
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
        PointerController.Instance.selectedUnit = null;
        ShootTurn();
        turnCount++;
        blueTurn = (turnCount % 2 == 0);
        currentTurnTime = maxTurnTime;
        
    }


    public void PurchaseObject(int cost)
    {
        if (turnCount % 2 == 0)
        {
            blueTurn = true;
            teamACash -= cost;
        }
        else
        {
            teamBCash -= cost;
            blueTurn = false;
        }

    }

    private void ShootTurn()
    {
        StartCoroutine(ManageShootTurnRoutine());
    }

    private IEnumerator ManageShootTurnRoutine()
    {
        shootTurn = true;
        Debug.Log("Shoot Turn Started");
        
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bullet").Length == 0);


        shootTurn = false;
        Debug.Log("Shoot Turn Ended");
    }
    

}
