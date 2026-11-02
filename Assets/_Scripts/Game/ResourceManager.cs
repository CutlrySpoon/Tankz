using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField]
    private SpawnableObject[] buildings;
    [SerializeField]
    private SpawnableObject[] units;
    [SerializeField]
    private SpawnableObject[] walls;
    [Space(10)]

    [Header("UI")]
    [SerializeField]
    private ResourceUI prefabUI;
    [SerializeField]
    private GameObject buildingsContainer;
    [SerializeField]
    private GameObject unitsContainer;
    [SerializeField]
    private GameObject wallsContainer;

    private void Start()
    {
        foreach (SpawnableObject building in buildings)
        {
            ResourceUI obj = Instantiate(prefabUI, buildingsContainer.transform);
            obj.SetUI(building);
        }

        foreach (SpawnableObject unit in units)
        {
            ResourceUI obj = Instantiate(prefabUI, unitsContainer.transform);
            obj.SetUI(unit);
        }

        foreach (SpawnableObject wall in walls)
        {
            ResourceUI obj = Instantiate(prefabUI, wallsContainer.transform);
            obj.SetUI(wall);
        }
    }
}
