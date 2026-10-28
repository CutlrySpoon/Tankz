using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager um
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("TileMapManager");
                instance = obj.AddComponent<UIManager>();
            }

            return instance;
        }
    }


    private void Awake()
    {
        instance = this;
    }

    //Smothly move UI to a specified position over time
    public IEnumerator Lerp(RectTransform obj, Vector2 startPos, Vector2 endPos, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            obj.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        obj.anchoredPosition = endPos;
    }
}
