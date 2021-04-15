using System;
using UnityEngine;
using UnityEngine.UI;

namespace RWS
{
    public class ControlsPanel : MonoBehaviour
    {
        [SerializeField]
        ControlListEntry throttleControlListEntry = null;

        [SerializeField]
        ControlListEntry rollControlListEntry = null;

        [SerializeField]
        ControlListEntry pitchControlListEntry = null;

        [SerializeField]
        ControlListEntry trimControlListEntry = null;

        [SerializeField]
        ControlListEntry viewControlListEntry = null;

        [SerializeField]
        ControlListEntry launchResetControlListEntry = null;

        [SerializeField]
        Button backButton = null;

        [SerializeField]
        Button saveButton = null;

        [SerializeField]
        Toggle axesDisplayToggle = null;

        [SerializeField]
        ScrollRect scrollRect = null;

        //----------------------------------------------------------------------------------------------------

        public Action OnBackButtonClicked;
        
        public void Show()
        {
            gameObject.SetActive( true );
            saveButton.gameObject.SetActive( false );

            throttleControlListEntry.BindingName = inputManager.ThrottleControl.BindingName ?? notDefinedName;
            throttleControlListEntry.Invert = inputManager.ThrottleControl.Invert;

            rollControlListEntry.BindingName = inputManager.RollControl.BindingName ?? notDefinedName;
            rollControlListEntry.Invert = inputManager.RollControl.Invert;

            pitchControlListEntry.BindingName = inputManager.PitchControl.BindingName ?? notDefinedName;
            pitchControlListEntry.Invert = inputManager.PitchControl.Invert;

            trimControlListEntry.BindingName = inputManager.TrimControl.BindingName ?? notDefinedName;
            trimControlListEntry.Invert = inputManager.TrimControl.Invert;

            viewControlListEntry.BindingName = inputManager.ViewControl.BindingName ?? notDefinedName;
            launchResetControlListEntry.BindingName = inputManager.LaunchResetControl.BindingName ?? notDefinedName;

            scrollRect.normalizedPosition = new Vector2( 0f, 1f );
        }

        public void Hide()
        {
            gameObject.SetActive( false );

            throttleControlListEntry.StopListening();
            rollControlListEntry.StopListening();
            pitchControlListEntry.StopListening();
            trimControlListEntry.StopListening();
            viewControlListEntry.StopListening();
            launchResetControlListEntry.StopListening();
        }

        //----------------------------------------------------------------------------------------------------

        readonly string notDefinedName = "Not defined";
        
        InputManager inputManager;
        ControlListEntry[] allControls;


        void Awake()
        {
            inputManager = InputManager.Instance;


            // Throttle

            throttleControlListEntry.BindingName = inputManager.ThrottleControl.BindingName ?? notDefinedName;
            throttleControlListEntry.OnStartListening += () =>
            {
                rollControlListEntry.StopListening();
                pitchControlListEntry.StopListening();
                trimControlListEntry.StopListening();
                viewControlListEntry.StopListening();
                launchResetControlListEntry.StopListening();

                inputManager.ListenAxis( control =>
                {
                    throttleControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                    throttleControlListEntry.StopListening();

                    inputManager.ThrottleControl.SetBinding( control );
                    saveButton.gameObject.SetActive( true );
                } );
            };
            throttleControlListEntry.OnStopListening += () => inputManager.StopAxisListening();
            throttleControlListEntry.Invert = inputManager.ThrottleControl.Invert;
            throttleControlListEntry.OnInvertChanged += value =>
            {
                inputManager.ThrottleControl.Invert = value;
                saveButton.gameObject.SetActive( true );
            };


            // Roll

            rollControlListEntry.BindingName = inputManager.RollControl.BindingName ?? notDefinedName;
            rollControlListEntry.OnStartListening += () =>
            {
                throttleControlListEntry.StopListening();
                pitchControlListEntry.StopListening();
                trimControlListEntry.StopListening();
                viewControlListEntry.StopListening();
                launchResetControlListEntry.StopListening();

                inputManager.ListenAxis( control =>
                {
                    rollControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                    rollControlListEntry.StopListening();

                    inputManager.RollControl.SetBinding( control );
                    saveButton.gameObject.SetActive( true );
                } );
            };
            rollControlListEntry.OnStopListening += () => inputManager.StopAxisListening();
            rollControlListEntry.Invert = inputManager.RollControl.Invert;
            rollControlListEntry.OnInvertChanged += value =>
            {
                inputManager.RollControl.Invert = value;
                saveButton.gameObject.SetActive( true );
            };


            // Pitch

            pitchControlListEntry.BindingName = inputManager.PitchControl.BindingName ?? notDefinedName;
            pitchControlListEntry.OnStartListening += () =>
            {
                throttleControlListEntry.StopListening();
                rollControlListEntry.StopListening();
                trimControlListEntry.StopListening();
                viewControlListEntry.StopListening();
                launchResetControlListEntry.StopListening();

                inputManager.ListenAxis( control =>
                {
                    pitchControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                    pitchControlListEntry.StopListening();

                    inputManager.PitchControl.SetBinding( control );
                    saveButton.gameObject.SetActive( true );
                } );
            };
            pitchControlListEntry.OnStopListening += () => inputManager.StopAxisListening();
            pitchControlListEntry.Invert = inputManager.PitchControl.Invert;
            pitchControlListEntry.OnInvertChanged += value =>
            {
                inputManager.PitchControl.Invert = value;
                saveButton.gameObject.SetActive( true );
            };


            // Trim

            trimControlListEntry.BindingName = inputManager.TrimControl.BindingName ?? notDefinedName;
            trimControlListEntry.OnStartListening += () =>
            {
                throttleControlListEntry.StopListening();
                rollControlListEntry.StopListening();
                pitchControlListEntry.StopListening();
                viewControlListEntry.StopListening();
                launchResetControlListEntry.StopListening();

                inputManager.ListenAxis( control =>
                {
                    trimControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                    trimControlListEntry.StopListening();

                    inputManager.TrimControl.SetBinding( control );
                    saveButton.gameObject.SetActive( true );
                } );
            };
            trimControlListEntry.OnStopListening += () => inputManager.StopAxisListening();
            trimControlListEntry.Invert = inputManager.TrimControl.Invert;
            trimControlListEntry.OnInvertChanged += value =>
            {
                inputManager.TrimControl.Invert = value;
                saveButton.gameObject.SetActive( true );
            };


            // Change view

            viewControlListEntry.BindingName = inputManager.ViewControl.BindingName ?? notDefinedName;
            viewControlListEntry.OnStartListening += () =>
            {
                throttleControlListEntry.StopListening();
                rollControlListEntry.StopListening();
                pitchControlListEntry.StopListening();
                trimControlListEntry.StopListening();
                launchResetControlListEntry.StopListening();

                inputManager.ListenButton( control =>
                {
                    viewControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                    viewControlListEntry.StopListening();

                    inputManager.ViewControl.SetBinding( control );
                    saveButton.gameObject.SetActive( true );
                } );
            };
            viewControlListEntry.OnStopListening += () => inputManager.StopButtonListening();


            // Launch / Reset

            launchResetControlListEntry.BindingName = inputManager.LaunchResetControl.BindingName ?? notDefinedName;
            launchResetControlListEntry.OnStartListening += () =>
            {
                throttleControlListEntry.StopListening();
                rollControlListEntry.StopListening();
                pitchControlListEntry.StopListening();
                trimControlListEntry.StopListening();
                viewControlListEntry.StopListening();

                inputManager.ListenButton( control =>
                {
                    launchResetControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                    launchResetControlListEntry.StopListening();

                    inputManager.LaunchResetControl.SetBinding( control );
                    saveButton.gameObject.SetActive( true );
                } );
            };
            launchResetControlListEntry.OnStopListening += () => inputManager.StopButtonListening();


            // . . .

            backButton.onClick.AddListener( OnBackButton );

            saveButton.onClick.AddListener( OnSaveButton );
            saveButton.gameObject.SetActive( false );

            axesDisplayToggle.isOn = inputManager.AxesDisplay;
            axesDisplayToggle.onValueChanged.AddListener( value => inputManager.AxesDisplay = value );


            // For easy enumeration

            allControls = new[]
            {
                throttleControlListEntry,
                rollControlListEntry,
                pitchControlListEntry,
                trimControlListEntry,
                viewControlListEntry,
                launchResetControlListEntry
            };
        }

        void OnEnable()
        {
            inputManager.OnEscapeButton += OnEscapeButton;
        }

        void OnDisable()
        {
            inputManager.OnEscapeButton -= OnEscapeButton;
        }
        

        void OnBackButton()
        {
            inputManager.LoadPlayerPrefs();
            OnBackButtonClicked?.Invoke();
        }

        void OnSaveButton()
        {
            saveButton.gameObject.SetActive( false );
            inputManager.SavePlayerPrefs();
        }

        void OnEscapeButton()
        {
            foreach( var control in allControls )
            {
                if( control.IsListening )
                {
                    control.StopListening();
                    return;
                }
            }

            OnBackButton();
        }
    }
}
