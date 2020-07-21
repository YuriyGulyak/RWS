using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SensitivityPanel : MonoBehaviour
{
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
    
    [SerializeField]
    Button saveButton = null;
    
    //--------------------------------------------------------------------------------------------------------------
    
    public void Show( Action onBackButtonCallback = null )
    {
        this.onBackButtonCallback = onBackButtonCallback;
        gameObject.SetActive( true );
        
        
        sensitivity.LoadPlayerPrefs();
        
        rollExpoSlider.value = sensitivity.RollExpo;
        rollSuperExpoSlider.value = sensitivity.RollSuperExpo;
        
        pitchExpoSlider.value = sensitivity.PitchExpo;
        pitchSuperExpoSlider.value = sensitivity.PitchSuperExpo;
        
        
        saveButton.gameObject.SetActive( false );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onBackButtonCallback = null;
    }
    
    //--------------------------------------------------------------------------------------------------------------

    ControlSensitivity sensitivity;
    Action onBackButtonCallback;
    
    
    void Awake()
    {
        rollExpoSlider.onValueChanged.AddListener( OnRollExpoSliderChanged );
        rollExpoInputField.onEndEdit.AddListener( OnRollExpoInputFieldChanged );

        rollSuperExpoSlider.onValueChanged.AddListener( OnRollSuperExpoSliderChanged );
        rollSuperExpoInputField.onEndEdit.AddListener( OnRollSuperExpoInputFieldChanged );


        pitchExpoSlider.onValueChanged.AddListener( OnPitchExpoSliderChanged );
        pitchExpoInputField.onEndEdit.AddListener( OnPitchExpoInputFieldChanged );

        pitchSuperExpoSlider.onValueChanged.AddListener( OnPitchSuperExpoSliderChanged );
        pitchSuperExpoInputField.onEndEdit.AddListener( OnPitchSuperExpoInputFieldChanged );
        
        
        sensitivity = new ControlSensitivity( true );
        
        rollExpoSlider.value = sensitivity.RollExpo;
        rollSuperExpoSlider.value = sensitivity.RollSuperExpo;
        
        pitchExpoSlider.value = sensitivity.PitchExpo;
        pitchSuperExpoSlider.value = sensitivity.PitchSuperExpo;
        
        
        backButton.onClick.AddListener( OnBackButton );
        
        saveButton.onClick.AddListener( OnSaveButton );
        saveButton.gameObject.SetActive( false );
    }

    
    void OnRollExpoSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        rollExpoInputField.text = FormatValue( newValue );

        sensitivity.RollExpo = newValue;
        
        UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
        
        saveButton.gameObject.SetActive( true );
    }
    
    void OnRollExpoInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        rollExpoSlider.value = newValue;
        rollExpoInputField.text = FormatValue( newValue );
    }
    
    void OnRollSuperExpoSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        rollSuperExpoInputField.text = FormatValue( newValue );

        sensitivity.RollSuperExpo = newValue;
        
        UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
        
        saveButton.gameObject.SetActive( true );
    }
    
    void OnRollSuperExpoInputFieldChanged( string newValueString )
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
        
        saveButton.gameObject.SetActive( true );
    }
    
    void OnPitchExpoInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        pitchExpoInputField.text = FormatValue( newValue );
        pitchExpoSlider.value = newValue;
    }
    
    void OnPitchSuperExpoSliderChanged( float newValue )
    {
        newValue = RoundValue( newValue );
        pitchSuperExpoInputField.text = FormatValue( newValue );

        sensitivity.PitchSuperExpo = newValue;
        
        UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
        
        saveButton.gameObject.SetActive( true );
    }
    
    void OnPitchSuperExpoInputFieldChanged( string newValueString )
    {
        var newValue = ParseValue( newValueString );
        
        newValue = Mathf.Clamp01( newValue );
        newValue = RoundValue( newValue );
        
        pitchSuperExpoInputField.text = FormatValue( newValue );
        pitchSuperExpoSlider.value = newValue;
    }

    
    void OnBackButton()
    {
        onBackButtonCallback?.Invoke();
    }

    void OnSaveButton()
    {
        sensitivity.SavePlayerPrefs();
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
