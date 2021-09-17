using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Events/FloatEvent" )]
public class FloatEvent : ScriptableObject, IGameEvent<float>
{
    List<IGameEventListener<float>> listeners = new List<IGameEventListener<float>>();

    public void Raise( float value )
    {
        for( var i = listeners.Count - 1; i >= 0; i-- )
        {
            listeners[ i ].OnEventRaised( value );
        }
    }

    public void RegisterListener( IGameEventListener<float> listener )
    {
        listeners.Add( listener );
    }

    public void UnregisterListener( IGameEventListener<float> listener )
    {
        listeners.Remove( listener );
    }
}