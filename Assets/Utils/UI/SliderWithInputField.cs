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

    [SerializeField]
    int fractionalDigits = 2;


    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }
    public FloatEvent OnValueChanged;

    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }

    public float MinValue { get; private set; }
    
    public float MaxValue { get; private set; }
    
    public bool IsFocused => inputField.isFocused;


    string valueFormat;
    
    
    void Awake()
    {
        MinValue = slider.minValue;
        MaxValue = slider.maxValue;
        
        valueFormat = inputField.text;
        inputField.text = RoundValue( slider.value, fractionalDigits ).ToString( valueFormat, CultureInfo.InvariantCulture );
        
        slider.onValueChanged.AddListener( OnSliderChanged );
        inputField.onEndEdit.AddListener( OnInputFieldChanged );
    }


    void OnSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue, fractionalDigits );
        inputField.text = newValue.ToString( valueFormat, CultureInfo.InvariantCulture );
        OnValueChanged.Invoke( newValue );
    }
    
    void OnInputFieldChanged( string newValueString )
    {
        var newValue = float.Parse( newValueString, CultureInfo.InvariantCulture );
        newValue = RoundValue( newValue, fractionalDigits );
        slider.value = RoundValue( newValue, fractionalDigits );
    }
    

    static float RoundValue( float value, int fractionalDigits )
    {
        return (float)System.Math.Round( value, fractionalDigits );
    }
}
