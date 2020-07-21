using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SensitivityPanel : MonoBehaviour
{
    [SerializeField]
    SliderWithInputField rollExpoSlider = null;

    [SerializeField]
    SliderWithInputField rollSuperExpoSlider = null;

    [SerializeField]
    SliderWithInputField pitchExpoSlider = null;

    [SerializeField]
    SliderWithInputField pitchSuperExpoSlider = null;

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
        rollExpoSlider.Value = sensitivity.RollExpo;
        rollSuperExpoSlider.Value = sensitivity.RollSuperExpo;
        pitchExpoSlider.Value = sensitivity.PitchExpo;
        pitchSuperExpoSlider.Value = sensitivity.PitchSuperExpo;
        
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
        rollExpoSlider.OnValueChanged.AddListener( OnRollExpoSliderChanged );
        rollSuperExpoSlider.OnValueChanged.AddListener( OnRollSuperExpoSliderChanged );
        pitchExpoSlider.OnValueChanged.AddListener( OnPitchExpoSliderChanged );
        pitchSuperExpoSlider.OnValueChanged.AddListener( OnPitchSuperExpoSliderChanged );
        
        sensitivity = new ControlSensitivity( true );
        rollExpoSlider.Value = sensitivity.RollExpo;
        rollSuperExpoSlider.Value = sensitivity.RollSuperExpo;
        pitchExpoSlider.Value = sensitivity.PitchExpo;
        pitchSuperExpoSlider.Value = sensitivity.PitchSuperExpo;

        backButton.onClick.AddListener( OnBackButton );
        saveButton.onClick.AddListener( OnSaveButton );
        saveButton.gameObject.SetActive( false );
    }

    
    void OnRollExpoSliderChanged( float newValue )
    {
        sensitivity.RollExpo = newValue;
        UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
        saveButton.gameObject.SetActive( true );
    }

    void OnRollSuperExpoSliderChanged( float newValue )
    {
        sensitivity.RollSuperExpo = newValue;
        UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
        saveButton.gameObject.SetActive( true );
    }

    void OnPitchExpoSliderChanged( float newValue )
    {
        sensitivity.PitchExpo = newValue;
        UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
        saveButton.gameObject.SetActive( true );
    }

    void OnPitchSuperExpoSliderChanged( float newValue )
    {
        sensitivity.PitchSuperExpo = newValue;
        UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
        saveButton.gameObject.SetActive( true );
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
}
