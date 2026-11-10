using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "Armoury - Place Action", menuName = "ScriptableObject/PlaceAction/Armoury")]
public class Object_Armoury : PlaceAction
{
    [SerializeField]
    private int power = 0;

    private bool isTeamA;

    public override void OnPlace(ObjectController obj)
    {
        if (gm.turnCount % 2 == 0)
        {
            gm.teamAPower += power;
            isTeamA = true;
        }

        else
        {
            gm.teamBPower += power;
            isTeamA = false;
        }

        Debug.Log("Armoury placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        if (isTeamA)
        {
            gm.teamAPower -= power;
        }

        else
        {
            gm.teamBPower -= power;
        }

        Debug.Log("Armoury destroyed");
    }
}
