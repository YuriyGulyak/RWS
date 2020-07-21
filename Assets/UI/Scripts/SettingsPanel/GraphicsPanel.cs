using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsPanel : MonoBehaviour
{
    [SerializeField]
    GraphicsManager graphicsManager = null;
    
    [SerializeField]
    TMP_Dropdown resolutionDropdown = null;

    [SerializeField]
    TMP_Dropdown qualityDropdown = null;

    [SerializeField]
    Toggle postProcessToggle = null;
    
    [SerializeField]
    Toggle vSyncToggle = null;

    [SerializeField]
    TMP_InputField displayInputField = null;
    
    [SerializeField]
    TMP_InputField maxFpsInputField = null;
    
    [SerializeField]
    Button cancelButton = null;
    
    [SerializeField]
    Button applyButton = null;
    
    //----------------------------------------------------------------------------------------------------
    
    public void Show( Action onCancelCallback = null )
    {
        this.onCancelCallback = onCancelCallback;
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        gameObject.SetActive( false );
        onCancelCallback = null;
    }
    
    //----------------------------------------------------------------------------------------------------

    Action onCancelCallback;


    void Awake()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions( new List<string>( Screen.resolutions.Select( resolution => $"{resolution.width} x {resolution.height}" ) ) );

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions( new List<string>( QualitySettings.names ) );
    }
}
