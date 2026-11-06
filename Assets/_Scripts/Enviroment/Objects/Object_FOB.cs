using UnityEngine;

[CreateAssetMenu(fileName = "FOB - Place Action", menuName = "ScriptableObject/PlaceAction/FOB")]
public class Object_FOB : PlaceAction
{
    public override void OnPlace(ObjectController obj)
    {
        Debug.Log("FOB placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        Debug.Log("FOB destroyed");
    }
}
