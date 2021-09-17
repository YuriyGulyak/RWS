using System;
using UnityEngine;
using UnityEngine.Events;

public class FloatEventListener : MonoBehaviour, IGameEventListener<float>
{
    public FloatEvent Event;

    [Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }
    public FloatUnityEvent Response;

    void OnEnable()
    {
        Event.RegisterListener( this );
    }

    void OnDisable()
    {
        Event.UnregisterListener( this );
    }

    public void OnEventRaised( float value )
    {
        Response.Invoke( value );
    }
}