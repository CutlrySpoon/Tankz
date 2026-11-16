using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "Lab - Place Action", menuName = "ScriptableObject/PlaceAction/Lab")]
public class Object_Lab : PlaceAction
{
    [SerializeField]
    private int research = 0;


    public override void OnPlace(ObjectController obj)
    {
        if (gm.turnCount % 2 == 0)
        {
            gm.teamAResearch += research;
        }

        else
        {
            gm.teamBResearch += research;
        }

        Debug.Log("Lab placed");
    }

    public override void OnObjectDestroyed(ObjectController obj)
    {
        if (obj.isTeamA)
        {
            gm.teamAResearch -= research;
        }

        else
        {
            gm.teamBResearch -= research;
        }

        Debug.Log("Lab destroyed");
    }
}
