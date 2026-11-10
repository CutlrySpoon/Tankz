using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using static ResourceManager;

// This is a UI prefab for displaying a given resource in the resource menu.

// By OS.

public class ResourceUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Image resourceSprite;
    [SerializeField]
    private TextMeshProUGUI resourceName;
    [SerializeField]
    private TextMeshProUGUI resourceCount;
    [SerializeField]
    private TextMeshProUGUI resourceCost;
    [SerializeField]
    private TextMeshProUGUI resourceDescription;
    [SerializeField]
    private GameObject dissabledScreen;
    [SerializeField]
    private TextMeshProUGUI dissabledText;
    [Space(5)]

    [SerializeField]
    private GameObject countObject;
    [SerializeField]
    private GameObject requirementImage;
    [SerializeField]
    private Sprite powerSprite;
    [SerializeField]
    private Sprite researchSprite;


    public ObjectData objectData { get; private set; }


    private int objectCountA = 0;
    private int maxObjectsA = 0;

    private int objectCountB = 0;
    private int maxObjectsB = 0;


    private void Start()
    {
        dissabledScreen.SetActive(false);
    }

    private void Update()
    {
        UpdateUI();
    }

    // Assign the UI to its given resource data and display it in the resource menu.
    public void SetUI(ObjectData obj)
    {
        this.objectData = obj;
        resourceSprite.sprite = obj.objectSprite;
        resourceSprite.color = Color.white;
        resourceSprite.SetNativeSize();
        resourceName.text = obj.objectName;
        resourceCost.text = $"$ {obj.objectCost}";
        resourceDescription.text = obj.objectDescription;

        if (obj.requiresPower)
        {
            countObject.SetActive(false);
            requirementImage.SetActive(true);

            requirementImage.GetComponent<Image>().sprite = powerSprite;
        }
        else if (obj.requiresResearch)
        {
            countObject.SetActive(false);
            requirementImage.SetActive(true);

            requirementImage.GetComponent<Image>().sprite = researchSprite;
        }
        else
        {
            countObject.SetActive(true);
            requirementImage.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        bool isTeamA = gm.turnCount % 2 == 0;

        int cash = isTeamA ? gm.teamACash : gm.teamBCash;
        int power = isTeamA ? gm.teamAPower : gm.teamBPower;
        int research = isTeamA ? gm.teamAResearch : gm.teamBResearch;
        int count = isTeamA ? objectCountA : objectCountB;

        bool notEnoughCash = cash < objectData.objectCost;

        int max = objectData.maxObjects;
        string blockedReason = count >= max ? "MAX REACHED": null;

        if (notEnoughCash)
            blockedReason = "NOT ENOUGH CASH";

        if (objectData.requiresPower)
        {
            if (power < 1)
            {
                blockedReason = "NOT ENOUGH POWER";
                max = 0;
            }                
        }

        else if (objectData.requiresResearch)
        {
            if (research < 1)
            {
                blockedReason = "NOT ENOUGH RESEARCH";
                max = 0;
            }                
        }

        if (isTeamA)
            maxObjectsA = max;
        else
            maxObjectsB = max;

        resourceCount.text = $"{count} / {max}";

        bool dissabled = notEnoughCash || count >= max;
        DissableButtion(dissabled, dissabled ? blockedReason : null);
    }

    private void DissableButtion(bool isDisabled, string text)
    {
        dissabledScreen.SetActive(isDisabled);
        dissabledText.text = $"[{text}]";
    }

    // Spawn the selected object, set its data and start following the pointer.
    public void OnClick()
    {
        if (gm.turnCount % 2 == 0)
        {
            if (objectCountA < maxObjectsA)
                rm.SpawnObject(objectData, this);
        }
        else
        {
            if (objectCountB < maxObjectsB)
                rm.SpawnObject(objectData, this);
        }
    }

    public void PlaceObject()
    {
        if (gm.turnCount % 2 == 0)
            objectCountA++;
        else
            objectCountB++;

    }
}