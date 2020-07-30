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


    string valueFormat;
    
    
    void Awake()
    {
        valueFormat = inputField.text;
        inputField.text = RoundValue( slider.value, fractionalDigits ).ToString( valueFormat, CultureInfo.InvariantCulture );
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
