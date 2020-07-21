using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderWithInputField : MonoBehaviour
{
    [SerializeField]
    Slider slider = null;
    
    [SerializeField]
    TMP_InputField inputField = null;


    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }
    public FloatEvent OnValueChanged;

    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }


    void OnEnable()
    {
        slider.onValueChanged.AddListener( OnSliderChanged );
        inputField.onEndEdit.AddListener( OnInputFieldChanged );
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveListener( OnSliderChanged );
        inputField.onEndEdit.RemoveListener( OnInputFieldChanged );
    }


    void OnSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        inputField.text = newValue.ToString( "0.00", CultureInfo.InvariantCulture );
        OnValueChanged.Invoke( newValue );
    }
    
    void OnInputFieldChanged( string newValueString )
    {
        var newValue = float.Parse( newValueString, CultureInfo.InvariantCulture );
        newValue = RoundValue( newValue );
        slider.value = RoundValue( newValue );
    }
    

    static float RoundValue( float value )
    {
        return (float)System.Math.Round( value, 2 );
    }
}
