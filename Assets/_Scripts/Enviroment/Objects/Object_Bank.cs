using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "Bank - Place Action", menuName = "ScriptableObject/PlaceAction/Bank")]
public class Object_Bank : PlaceAction
{
    [SerializeField]
    private int income = 0;

    private bool isTeamA;

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

        Debug.Log("Bank1 placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        if (isTeamA)
        {
            gm.teamAIncome -= income;
        }

        else
        {
            gm.teamBIncome -= income;
        }

        Debug.Log("Bank1 destroyed");
    }
}
