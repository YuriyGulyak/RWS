using System;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class SoundPanel : MonoBehaviour
    {
        [SerializeField]
        SoundManager soundManager = null;

        [SerializeField]
        SliderWithInputField masterVolumeSlider = null;

        [SerializeField]
        SliderWithInputField motorVolumeSlider = null;

        [SerializeField]
        SliderWithInputField servoVolumeSlider = null;

        [SerializeField]
        SliderWithInputField buzzerVolumeSlider = null;

        [SerializeField]
        SliderWithInputField windVolumeSlider = null;

        [SerializeField]
        Button backButton = null;

        [SerializeField]
        Button applyButton = null;

        //----------------------------------------------------------------------------------------------------

        public void Show( Action onBackButtonCallback = null )
        {
            this.onBackButtonCallback = onBackButtonCallback;
            gameObject.SetActive( true );

            masterVolumeSlider.Value = soundManager.MasterVolume;
            motorVolumeSlider.Value = soundManager.MotorVolume;
            servoVolumeSlider.Value = soundManager.ServoVolume;
            buzzerVolumeSlider.Value = soundManager.BuzzerVolume;
            windVolumeSlider.Value = soundManager.WindVolume;

            applyButton.gameObject.SetActive( false );
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            onBackButtonCallback = null;
        }

        //----------------------------------------------------------------------------------------------------

        Action onBackButtonCallback;


        void OnValidate()
        {
            if( !soundManager )
            {
                soundManager = SoundManager.Instance;
            }
        }

        void Awake()
        {
            masterVolumeSlider.OnValueChanged.AddListener( OnMasterVolumeSliderChanged );
            motorVolumeSlider.OnValueChanged.AddListener( OnMotorVolumeSliderChanged );
            servoVolumeSlider.OnValueChanged.AddListener( OnServoVolumeSliderChanged );
            buzzerVolumeSlider.OnValueChanged.AddListener( OnBuzzerVolumeSliderChanged );
            windVolumeSlider.OnValueChanged.AddListener( OnWindVolumeSliderChanged );

            backButton.onClick.AddListener( OnBackButton );

            applyButton.onClick.AddListener( OnApplyButton );
            applyButton.gameObject.SetActive( false );

            InputManager.Instance.OnEscapeButton += OnEscapeButton;
        }


        void OnMasterVolumeSliderChanged( float newValue )
        {
            applyButton.gameObject.SetActive( true );
        }

        void OnMotorVolumeSliderChanged( float newValue )
        {
            applyButton.gameObject.SetActive( true );
        }

        void OnServoVolumeSliderChanged( float newValue )
        {
            applyButton.gameObject.SetActive( true );
        }

        void OnBuzzerVolumeSliderChanged( float newValue )
        {
            applyButton.gameObject.SetActive( true );
        }

        void OnWindVolumeSliderChanged( float newValue )
        {
            applyButton.gameObject.SetActive( true );
        }


        void OnBackButton()
        {
            onBackButtonCallback?.Invoke();
            Hide();
        }

        void OnApplyButton()
        {
            soundManager.MasterVolume = masterVolumeSlider.Value;
            soundManager.MotorVolume = motorVolumeSlider.Value;
            soundManager.ServoVolume = servoVolumeSlider.Value;
            soundManager.BuzzerVolume = buzzerVolumeSlider.Value;
            soundManager.WindVolume = windVolumeSlider.Value;
            soundManager.SavePlayerPrefs();

            applyButton.gameObject.SetActive( false );
        }

        void OnEscapeButton()
        {
            if( masterVolumeSlider.IsFocused || motorVolumeSlider.IsFocused || servoVolumeSlider.IsFocused ||
                buzzerVolumeSlider.IsFocused || windVolumeSlider.IsFocused )
            {
                return;
            }

            OnBackButton();
        }
    }
}