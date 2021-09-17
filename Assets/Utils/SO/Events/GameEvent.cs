using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Events/GameEvent" )]
public class GameEvent : ScriptableObject, IGameEvent
{
    List<IGameEventListener> listeners = new List<IGameEventListener>();

    public void Raise()
    {
        for( var i = listeners.Count - 1; i >= 0; i-- )
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener( IGameEventListener listener )
    {
        listeners.Add(listener);
    }

    public void UnregisterListener( IGameEventListener listener )
    {
        listeners.Remove(listener);
    }
}
