using UnityEngine;

public abstract class PlaceAction : ScriptableObject
{
    public abstract void OnPlace(ObjectController obj);

    public virtual void OnObjectDestroyed(ObjectController obj) { }
}