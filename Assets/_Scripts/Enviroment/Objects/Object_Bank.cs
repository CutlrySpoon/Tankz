using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "Bank - Place Action", menuName = "ScriptableObject/PlaceAction/Bank")]
public class Object_Bank : PlaceAction
{
    [SerializeField]
    private int income = 0;

    public override void OnPlace(ObjectController obj)
    {
        if (gm.turnCount % 2 == 0)
        {
            gm.teamAIncome += income;
        }

        else
        {
            gm.teamBIncome += income;
        }

        Debug.Log("Bank placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        if (obj.isTeamA)
        {
            gm.teamAIncome -= income;
        }

        else
        {
            gm.teamBIncome -= income;
        }

        Debug.Log("Bank destroyed");
    }
}
