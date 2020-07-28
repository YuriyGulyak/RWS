using System;
using RWS;
using UnityEngine;
using UnityEngine.UI;

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
    ControlListEntry launchControlListEntry = null;

    [SerializeField]
    ControlListEntry resetControlListEntry = null;
    
    [SerializeField]
    Button backButton = null;

    [SerializeField]
    Button saveButton = null;

    [SerializeField]
    Toggle axesDisplayToggle = null;

    [SerializeField]
    ScrollRect scrollRect = null;
    
    //----------------------------------------------------------------------------------------------------

    public void Show( Action onBackButtonCallback = null )
    {
        this.onBackButtonCallback = onBackButtonCallback;
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

        launchControlListEntry.BindingName = inputManager.LaunchControl.BindingName ?? notDefinedName;
        resetControlListEntry.BindingName = inputManager.ResetControl.BindingName ?? notDefinedName;
        
        scrollRect.normalizedPosition = new Vector2( 0f, 1f );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onBackButtonCallback = null;
        
        throttleControlListEntry.StopListening();
        rollControlListEntry.StopListening();
        pitchControlListEntry.StopListening();
        trimControlListEntry.StopListening();
        launchControlListEntry.StopListening();
        resetControlListEntry.StopListening();
    }
    
    //----------------------------------------------------------------------------------------------------
    
    readonly string notDefinedName = "Not defined";
    
    Action onBackButtonCallback;
    InputManager inputManager;

    
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
            launchControlListEntry.StopListening();
            resetControlListEntry.StopListening();
            
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
            launchControlListEntry.StopListening();
            resetControlListEntry.StopListening();
            
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
            launchControlListEntry.StopListening();
            resetControlListEntry.StopListening();
            
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
            launchControlListEntry.StopListening();
            resetControlListEntry.StopListening();
            
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
        
        
        // Launch
        
        launchControlListEntry.BindingName = inputManager.LaunchControl.BindingName ?? notDefinedName;
        launchControlListEntry.OnStartListening += () =>
        {
            throttleControlListEntry.StopListening();
            rollControlListEntry.StopListening();
            pitchControlListEntry.StopListening();
            trimControlListEntry.StopListening();
            resetControlListEntry.StopListening();
            
            inputManager.ListenButton( control =>
            {
                launchControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                launchControlListEntry.StopListening();

                inputManager.LaunchControl.SetBinding( control );
                saveButton.gameObject.SetActive( true );
            } );
        };
        launchControlListEntry.OnStopListening += () => inputManager.StopButtonListening();


        // Reset
        
        resetControlListEntry.BindingName = inputManager.ResetControl.BindingName ?? notDefinedName;
        resetControlListEntry.OnStartListening += () =>
        {
            throttleControlListEntry.StopListening();
            rollControlListEntry.StopListening();
            pitchControlListEntry.StopListening();
            trimControlListEntry.StopListening();
            launchControlListEntry.StopListening();
            
            inputManager.ListenButton( control =>
            {
                resetControlListEntry.BindingName = $"{control.device.displayName}: {control.displayName}";
                resetControlListEntry.StopListening();

                inputManager.ResetControl.SetBinding( control );
                saveButton.gameObject.SetActive( true );
            } );
        };
        resetControlListEntry.OnStopListening += () => inputManager.StopButtonListening();
        
        
        // . . .
        
        backButton.onClick.AddListener( OnBackButton );

        saveButton.onClick.AddListener( OnSaveButton );
        saveButton.gameObject.SetActive( false );

        axesDisplayToggle.isOn = inputManager.AxesDisplay;
        axesDisplayToggle.onValueChanged.AddListener( value => inputManager.AxesDisplay = value );
    }
    

    void OnBackButton()
    {
        inputManager.LoadPlayerPrefs();
        onBackButtonCallback?.Invoke();
        Hide();
    }

    void OnSaveButton()
    {
        saveButton.gameObject.SetActive( false );
        inputManager.SavePlayerPrefs();
    }
}
