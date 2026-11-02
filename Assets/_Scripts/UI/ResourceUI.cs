using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField]
    private Image resourceSprite;
    [SerializeField]
    private TextMeshProUGUI resourceName;
    [SerializeField]
    private TextMeshProUGUI resourceCost;
    [SerializeField]
    private TextMeshProUGUI resourceDescription;
    [Space(10)]

    private SpawnableObject obj;

    public void SetUI(SpawnableObject obj)
    {
        this.obj = obj;
        resourceSprite.sprite = obj.objectSprite;
        resourceName.text = obj.objectName;
        resourceCost.text = $"$ {obj.objectCost}";
        resourceDescription.text = obj.objectDescription;
    }

    public void SpawnObject()
    {
        
    }
}