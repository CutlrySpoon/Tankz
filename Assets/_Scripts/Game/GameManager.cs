using System.Collections;
using System.Collections.Generic;
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
    public bool shootTurn = false;

    public event System.Action OnTurnChange;
    private int TurnCount;


    public bool blueTurn = true;
    //Logic
    float elapsedTime = 0;



    private void Awake()
    {
        instance = this;
    }

    // Reset timer
    private void Start()
    {
        blueTurn = true;
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
            blueTurn = true;
            teamACash += teamAIncome;
        }
        else
        {
            blueTurn = false;
            teamBCash += teamBIncome;
        }
    }

    public void EndTurn()
    {
        inPlay = false;
        if (PointerController.Instance != null)
        {
            PointerController.Instance.selectedUnit = null;
        }

        StartCoroutine(ManageShootTurn());
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

    public struct QueuedShot
    {
        public GameObject prefab;
        public Vector3 position;
        public Quaternion rotation;
        public float strength;
    }


    private Queue<QueuedShot> shotQueue = new Queue<QueuedShot>();

    public void QueueShot(GameObject prefab, Vector3 pos, Quaternion rot, float strength)
    {
        shotQueue.Enqueue(new QueuedShot { prefab = prefab, position = pos, rotation = rot, strength = strength });
    }



    private IEnumerator ManageShootTurn()
    {
        PlayMenuManager.instance.turnScreen.SetActive(false);
        
        shootTurn = true;
        while (shotQueue.Count > 0)
        {
            QueuedShot shot = shotQueue.Dequeue();
            GameObject bullet = Instantiate(shot.prefab, shot.position, shot.rotation);
            BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
            if (bulletScript != null)
            {
                bulletScript.shotStregth = shot.strength;
            }

            yield return new WaitUntil(() => bullet == null);
            yield return new WaitForSeconds(1f);
        }
        
        turnCount++;
        blueTurn = (turnCount % 2 == 0);
        currentTurnTime = maxTurnTime;
        shootTurn = false;
        Debug.Log("Shoot Turn Ended");
        PlayMenuManager.instance.SetUI();
        PlayMenuManager.instance.turnScreen.SetActive(true);
    }


}
