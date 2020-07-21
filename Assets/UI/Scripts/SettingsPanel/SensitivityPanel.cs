using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SensitivityPanel : MonoBehaviour
{
    //[SerializeField]
    //RateProfile rateProfile = null;

    
    [SerializeField]
    Slider rollExpoSlider = null;
    
    [SerializeField]
    TMP_InputField rollExpoInputField = null;

    [SerializeField]
    Slider rollSuperExpoSlider = null;
    
    [SerializeField]
    TMP_InputField rollSuperExpoInputField = null;
    
    
    [SerializeField]
    Slider pitchExpoSlider = null;
    
    [SerializeField]
    TMP_InputField pitchExpoInputField = null;

    [SerializeField]
    Slider pitchSuperExpoSlider = null;
    
    [SerializeField]
    TMP_InputField pitchSuperExpoInputField = null;
    

    [SerializeField]
    RectTransform curvesPanelRect = null;
    
    [SerializeField]
    UILineRenderer rollLineRenderer = null;

    [SerializeField]
    UILineRenderer pitchLineRenderer = null;
    
    
    [SerializeField]
    Button backButton = null;
    
    //--------------------------------------------------------------------------------------------------------------
    
    public void Show( Action onBackButtonCallback = null )
    {
        this.onBackButtonCallback = onBackButtonCallback;
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onBackButtonCallback = null;
    }
    
    //--------------------------------------------------------------------------------------------------------------

    ControllerSensitivity sensitivity;
    Action onBackButtonCallback;
    
    
    void Awake()
    {
        sensitivity = new ControllerSensitivity();
        sensitivity.LoadPlayerPrefs();
        
        
        rollExpoSlider.onValueChanged.AddListener( OnRollExpoSliderChanged );
        rollExpoInputField.onEndEdit.AddListener( OnRollExpoInputFieldChanged );

        rollSuperExpoSlider.onValueChanged.AddListener( OnRollSuperRateSliderChanged );
        rollSuperExpoInputField.onEndEdit.AddListener( OnRollSuperRateInputFieldChanged );

        rollExpoSlider.value = sensitivity.RollExpo;
        rollSuperExpoSlider.value = sensitivity.RollSuperExpo;
        
        
        pitchExpoSlider.onValueChanged.AddListener( OnPitchExpoSliderChanged );
        pitchExpoInputField.onEndEdit.AddListener( OnPitchExpoInputFieldChanged );

        pitchSuperExpoSlider.onValueChanged.AddListener( OnPitchSuperRateSliderChanged );
        pitchSuperExpoInputField.onEndEdit.AddListener( OnPitchSuperRateInputFieldChanged );

        pitchExpoSlider.value = sensitivity.PitchExpo;
        pitchSuperExpoSlider.value = sensitivity.PitchSuperExpo;
        
        backButton.onClick.AddListener( OnBackButton );
        
        //UpdateCurve( rollLineRenderer, ( value ) => rateProfile.EvaluateRoll( value ) );
        //UpdateCurve( rollLineRenderer, ( value ) => rateProfile.EvaluatePitch( value ) );
    }


    void OnRollExpoSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        rollExpoInputField.text = FormatValue( newValue );

        sensitivity.RollExpo = newValue;
        UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
    }
    
    void OnRollExpoInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        rollExpoSlider.value = newValue;
        rollExpoInputField.text = FormatValue( newValue );
    }
    
    void OnRollSuperRateSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        rollSuperExpoInputField.text = FormatValue( newValue );

        sensitivity.RollSuperExpo = newValue;
        UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
    }
    
    void OnRollSuperRateInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        rollSuperExpoInputField.text = FormatValue( newValue );
        rollSuperExpoSlider.value = newValue;
    }
   
   
    void OnPitchExpoSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        pitchExpoInputField.text = FormatValue( newValue );

        sensitivity.PitchExpo = newValue;
        UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
    }
    
    void OnPitchExpoInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        pitchExpoInputField.text = FormatValue( newValue );
        pitchExpoSlider.value = newValue;
    }
    
    void OnPitchSuperRateSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        pitchSuperExpoInputField.text = FormatValue( newValue );

        sensitivity.PitchSuperExpo = newValue;
        UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
    }
    
    void OnPitchSuperRateInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        pitchSuperExpoInputField.text = FormatValue( newValue );
        pitchSuperExpoSlider.value = newValue;
    }


    void UpdateCurve( UILineRenderer lineRenderer, Func<float, float> evaluateFunc )
    {
        var rectSize = curvesPanelRect.sizeDelta;
        var curveResolution = 50;
        var newCurvePoints = new Vector2[ curveResolution ];

        for( var i = 0; i < newCurvePoints.Length; i++ )
        {
            newCurvePoints[ i ] = new Vector2
            {
                x = ( rectSize.x / ( curveResolution - 1 ) ) * i,
                y = evaluateFunc( Mathf.Lerp( -1f, 1f, (float)i / ( curveResolution - 1 ) ) ) * rectSize.y / 2f
            };
        }
        
        // Need set new array, updating the existing array points not working
        lineRenderer.Points = newCurvePoints;
    }


    void OnBackButton()
    {
        sensitivity.SavePlayerPrefs();
        onBackButtonCallback?.Invoke();
    }


    static float ParseValue( string value )
    {
        return float.Parse( value, CultureInfo.InvariantCulture );
    }

    static float RoundValue( float value )
    {
        return (float)Math.Round( value, 2 );
    }

    static string FormatValue( float value )
    {
        return value.ToString( "0.00", CultureInfo.InvariantCulture );
    }
}
