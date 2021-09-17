using UnityEngine;

public class TestFloatEvent : MonoBehaviour, IGameEventListener<float>
{
    [SerializeField]
    FloatEvent _floatEvent = default;


    void OnEnable()
    {
        _floatEvent.RegisterListener( this );
    }

    void OnDisable()
    {
        _floatEvent.UnregisterListener( this );
    }


    public void OnFloatEvent( float value )
    {
        print( value );
    }

    public void OnEventRaised( float value )
    {
        print( value );
    }
}
