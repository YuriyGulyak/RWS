using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseVariable<T> : ScriptableObject
{
    [SerializeField]
    T _value;

    public T Value
    {
        get => _value;
        set
        {
            var valueChanged = !_value.Equals( value );

            _value = value;
            
            if( valueChanged )
            {
                OnValueChanged.Invoke( _value );
            }
        }
    }

    [Serializable]
    public class TEvent : UnityEvent<T> {}
    public TEvent OnValueChanged;
}