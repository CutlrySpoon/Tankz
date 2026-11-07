using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "FOB - Place Action", menuName = "ScriptableObject/PlaceAction/FOB")]
public class Object_FOB : PlaceAction
{
    public override void OnPlace(ObjectController obj)
    {
        if (gm.turnCount % 2 == 0)
        {
            gm.teamAPower += 2;
            gm.teamAResearch += 2;
        }

        else
        {
            gm.teamBPower += 2;
            gm.teamBResearch += 2;
        }

        Debug.Log("FOB placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        Debug.Log("FOB destroyed");
    }
}
