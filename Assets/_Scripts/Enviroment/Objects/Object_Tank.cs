using UnityEngine;

[CreateAssetMenu(fileName = "Tank - Place Action", menuName = "ScriptableObject/PlaceAction/Tank")]
public class Object_Tank : PlaceAction
{
    [SerializeField] private GameObject tankPrefab;
    public override void OnPlace(ObjectController obj)
    {
        GameObject tank = Instantiate(tankPrefab, obj.transform.position, obj.transform.rotation);
        UnitHealth health = tank.GetComponent<UnitHealth>();

        health.SetHealthBar(obj.objectData.healthBar);
        health.SetHealthUI(obj.objectData.objectSize.y);

        Destroy(obj.gameObject);
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {

    }
}
