using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "FOB - Place Action", menuName = "ScriptableObject/PlaceAction/FOB")]
public class Object_FOB : PlaceAction
{
    [SerializeField]
    private int income = 0;
    [SerializeField]
    private int power = 0;
    [SerializeField]
    private int research = 0;

    public override void OnPlace(ObjectController obj)
    {
        if (gm.turnCount % 2 == 0)
        {
            gm.teamAIncome += income;
            gm.teamAPower += power;
            gm.teamAResearch += research;
        }

        else
        {
            gm.teamBIncome += income;
            gm.teamBPower += power;
            gm.teamBResearch += research;
        }

        Debug.Log("FOB placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        gm.EndGame(obj.isTeamA);

        Debug.Log("FOB destroyed");
    }
}
