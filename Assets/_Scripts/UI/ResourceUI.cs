using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    private TextMeshProUGUI resourceCost;
    [SerializeField]
    private TextMeshProUGUI resourceDescription;

    public ObjectData objectData { get; private set; }

    // Assign the UI to its given resource data and display it in the resource menu.
    public void SetUI(ObjectData obj)
    {
        this.objectData = obj;
        resourceSprite.sprite = obj.objectSprite;
        resourceName.text = obj.objectName;
        resourceCost.text = $"$ {obj.objectCost}";
        resourceDescription.text = obj.objectDescription;
    }

    // Spawn the selected object, set its data and start following the pointer.
    public void OnClick()
    {
        rm.SpawnObject(objectData);
    }
}