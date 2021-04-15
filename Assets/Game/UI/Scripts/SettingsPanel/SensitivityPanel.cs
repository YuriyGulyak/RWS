using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace RWS
{
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

        public Action OnBackButtonClicked;
        
        public void Show()
        {
            gameObject.SetActive( true );

            sensitivity.LoadPlayerPrefs();
            
            rollExpoSlider.Value = sensitivity.RollExpo * 100f;
            rollSuperExpoSlider.Value = sensitivity.RollSuperExpo * 100f;
            pitchExpoSlider.Value = sensitivity.PitchExpo * 100f;
            pitchSuperExpoSlider.Value = sensitivity.PitchSuperExpo * 100f;

            saveButton.gameObject.SetActive( false );
        }

        public void Hide()
        {
            gameObject.SetActive( false );
        }

        //--------------------------------------------------------------------------------------------------------------

        ControlSensitivity sensitivity;
        InputManager inputManager;


        void Awake()
        {
            sensitivity = new ControlSensitivity( true );

            rollExpoSlider.OnValueChanged.AddListener( OnRollExpoSliderChanged );
            rollSuperExpoSlider.OnValueChanged.AddListener( OnRollSuperExpoSliderChanged );
            pitchExpoSlider.OnValueChanged.AddListener( OnPitchExpoSliderChanged );
            pitchSuperExpoSlider.OnValueChanged.AddListener( OnPitchSuperExpoSliderChanged );

            backButton.onClick.AddListener( OnBackButton );
            saveButton.onClick.AddListener( OnSaveButton );
            saveButton.gameObject.SetActive( false );

            inputManager = InputManager.Instance;
        }

        void Start()
        {
            rollExpoSlider.Value = sensitivity.RollExpo * 100f;
            rollSuperExpoSlider.Value = sensitivity.RollSuperExpo * 100f;
            pitchExpoSlider.Value = sensitivity.PitchExpo * 100f;
            pitchSuperExpoSlider.Value = sensitivity.PitchSuperExpo * 100f;
        }

        void OnEnable()
        {
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;
        }


        void OnRollExpoSliderChanged( float newValue )
        {
            sensitivity.RollExpo = newValue / 100f;
            UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
            saveButton.gameObject.SetActive( true );
        }

        void OnRollSuperExpoSliderChanged( float newValue )
        {
            sensitivity.RollSuperExpo = newValue / 100f;
            UpdateCurve( rollLineRenderer, ( value ) => sensitivity.EvaluateRoll( value ) );
            saveButton.gameObject.SetActive( true );
        }

        void OnPitchExpoSliderChanged( float newValue )
        {
            sensitivity.PitchExpo = newValue / 100f;
            UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
            saveButton.gameObject.SetActive( true );
        }

        void OnPitchSuperExpoSliderChanged( float newValue )
        {
            sensitivity.PitchSuperExpo = newValue / 100f;
            UpdateCurve( pitchLineRenderer, ( value ) => sensitivity.EvaluatePitch( value ) );
            saveButton.gameObject.SetActive( true );
        }

        void OnBackButton()
        {
            OnBackButtonClicked?.Invoke();
        }

        void OnSaveButton()
        {
            sensitivity.SavePlayerPrefs();
            saveButton.gameObject.SetActive( false );
        }

        void OnEscapeButton()
        {
            if( rollExpoSlider.IsFocused || rollSuperExpoSlider.IsFocused || pitchExpoSlider.IsFocused || pitchSuperExpoSlider.IsFocused )
            {
                return;
            }

            OnBackButton();
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
}