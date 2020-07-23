// !! Need set execution order after default time fot this script !!

using Boo.Lang;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsManager : MonoBehaviour
{
    [SerializeField]
    DisplayManger displayManger = null;

    [SerializeField]
    PostProcessVolume postProcessVolume = null;

    //----------------------------------------------------------------------------------------------------

    public Resolution[] Resolutions => resolutions;

    public Resolution Resolution
    {
        get
        {
            var resolutionWithoutRefreshRate = new Resolution
            {
                width = Screen.currentResolution.width, 
                height = Screen.currentResolution.height
            };
            return resolutionWithoutRefreshRate;
        }
        set => Screen.SetResolution( value.width, value.height, true );
    }

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
        }
    }

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


    public void LoadAndApllyPlayerPrefs()
    {
        if( PlayerPrefs.HasKey( qualityLevelKey ) )
        {
            QualityLevel = PlayerPrefs.GetInt( qualityLevelKey );
        }
        
        if( PlayerPrefs.HasKey( postProcessKey ) )
        {
            PostProcess = PlayerPrefs.GetInt( postProcessKey ) > 0;
        }
        
        if( PlayerPrefs.HasKey( targetDisplayKey ) )
        {
            TargetDisplay = PlayerPrefs.GetInt( targetDisplayKey );
        }

        if( PlayerPrefs.HasKey( vSyncKey ) )
        {
            VSync = PlayerPrefs.GetInt( vSyncKey ) > 0;
        }

        if( PlayerPrefs.HasKey( fpsLimitKey ) )
        {
            FpsLimit = PlayerPrefs.GetInt( fpsLimitKey );
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

    Resolution[] resolutions;
    int targetDisplay;
    int fpsLimit;

    
    void Awake()
    {
        var resolutionList = new List<Resolution>();
        foreach( var resolution in Screen.resolutions )
        {
            var resolutionWithoutRefreshRate = new Resolution
            {
                width = resolution.width, 
                height = resolution.height
            };
            if( !resolutionList.Contains( resolutionWithoutRefreshRate ) )
            {
                resolutionList.Add( resolutionWithoutRefreshRate );
            }
        }
        resolutions = resolutionList.ToArray();
        
        LoadAndApllyPlayerPrefs();
    }
}