// !! Need set execution order after default time fot this script !!

using System;
using Boo.Lang;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsManager : Singleton<GraphicsManager>
{
    [SerializeField]
    DisplayManger displayManger = null;

    [SerializeField]
    PostProcessVolume postProcessVolume = null;

    //----------------------------------------------------------------------------------------------------

    public string[] QualityNames => QualitySettings.names;

    public int QualityLevel
    {
        get => QualitySettings.GetQualityLevel();
        set => QualitySettings.SetQualityLevel( value );
    }

    public bool PostProcess
    {
        get => postProcessVolume.enabled;
        set => postProcessVolume.enabled = value;
    }

    public int DisplayCount => displayManger.GetDisplays().Length;
    
    public int TargetDisplay
    {
        get => targetDisplay;
        set
        {
            if( value.Equals( targetDisplay ) )
            {
                return;
            }
            targetDisplay = value;
            displayManger.SetTargetDisplay( targetDisplay );
            OnTargetDisplayChenged?.Invoke( targetDisplay );
        }
    }

    public Action<int> OnTargetDisplayChenged;
    
    public bool VSync
    {
        get => QualitySettings.vSyncCount == 1;
        set => QualitySettings.vSyncCount = value ? 1 : 0;
    }

    public int FpsLimit
    {
        get => fpsLimit;
        set
        {
            fpsLimit = value;
            Application.targetFrameRate = fpsLimit;
        }
    }


    public Resolution GetResolution()
    {
        var resolutionWithoutRefreshRate = new Resolution
        {
            width = Screen.currentResolution.width, 
            height = Screen.currentResolution.height
        };
        return resolutionWithoutRefreshRate;
    }

    public void SetResolution( Resolution resolution )
    {
        Screen.SetResolution( resolution.width, resolution.height, true );
    }

    public List<Resolution> GetResolutions()
    {
        var resolutions = new List<Resolution>();
        foreach( var resolution in Screen.resolutions )
        {
            var resolutionWithoutRefreshRate = new Resolution
            {
                width = resolution.width, 
                height = resolution.height
            };
            if( !resolutions.Contains( resolutionWithoutRefreshRate ) )
            {
                resolutions.Add( resolutionWithoutRefreshRate );
            }
        }
        return resolutions;
    }

    public void LoadAndApllyPlayerPrefs()
    {
        if( PlayerPrefs.HasKey( qualityLevelKey ) )
        {
            QualityLevel = PlayerPrefs.GetInt( qualityLevelKey );
        }
        else
        {
            QualityLevel = 2;
        }
        
        if( PlayerPrefs.HasKey( postProcessKey ) )
        {
            PostProcess = PlayerPrefs.GetInt( postProcessKey ) > 0;
        }
        else
        {
            PostProcess = true;
        }
        
        if( PlayerPrefs.HasKey( targetDisplayKey ) )
        {
            TargetDisplay = PlayerPrefs.GetInt( targetDisplayKey );
        }

        if( PlayerPrefs.HasKey( vSyncKey ) )
        {
            VSync = PlayerPrefs.GetInt( vSyncKey ) > 0;
        }
        else
        {
            VSync = true;
        }

        if( PlayerPrefs.HasKey( fpsLimitKey ) )
        {
            FpsLimit = PlayerPrefs.GetInt( fpsLimitKey );
        }
        else
        {
            FpsLimit = 60;
        }
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt( qualityLevelKey, QualityLevel );
        PlayerPrefs.SetInt( postProcessKey, PostProcess ? 1 : 0 );
        PlayerPrefs.SetInt( targetDisplayKey, TargetDisplay );
        PlayerPrefs.SetInt( vSyncKey, VSync ? 1 : 0 );
        PlayerPrefs.SetInt( fpsLimitKey, FpsLimit );
    }

    //----------------------------------------------------------------------------------------------------

    readonly string qualityLevelKey = "QualityLevel";
    readonly string postProcessKey = "PostProcess";
    readonly string targetDisplayKey = "TargetDisplay";
    readonly string vSyncKey = "VSync";
    readonly string fpsLimitKey = "FpsLimit";

    int targetDisplay;
    int fpsLimit;

    
    void Awake()
    {
        LoadAndApllyPlayerPrefs();
    }
}