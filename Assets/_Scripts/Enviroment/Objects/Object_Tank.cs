using UnityEngine;

[CreateAssetMenu(fileName = "Tank - Place Action", menuName = "ScriptableObject/PlaceAction/Tank")]
public class Object_Tank : PlaceAction
{
    [SerializeField] private GameObject tankPrefab;
    public override void OnPlace(ObjectController obj)
    {
        Instantiate(tankPrefab, obj.transform.position, obj.transform.rotation);
        Destroy(obj.gameObject);

    }

    public override void OnObjectDestroyed(ObjectController obj)
    {

    }
}
