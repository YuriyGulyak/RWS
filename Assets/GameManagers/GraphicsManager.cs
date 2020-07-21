using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsManager : MonoBehaviour
{
    [SerializeField]
    DisplayManger displayManger = null;

    [SerializeField]
    PostProcessVolume postProcessVolume = null;
    
    //----------------------------------------------------------------------------------------------------
    
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
            
            PlayerPrefs.SetInt( targetDisplayKey, targetDisplay );
        }
    }
    
    
    public Resolution[] GetResolutions()
    {
        return null;
    }

    public void SetResolution( Resolution resolution )
    {
        
    }


    public bool PostProcess
    {
        get => postProcessEnabled;
        set
        {
            if( value.Equals( postProcessEnabled ) )
            {
                return;
            }
            postProcessEnabled = value;
            
            postProcessVolume.enabled = value;
            
            PlayerPrefs.SetInt( postProcessKey, postProcessEnabled ? 1 : 0 );
        }
    }

    //----------------------------------------------------------------------------------------------------

    readonly string targetDisplayKey = "TargetDisplay";
    readonly string postProcessKey = "PostProcess";
    
    int targetDisplay = 0;
    bool postProcessEnabled = true;

    
    void Awake()
    {
        if( PlayerPrefs.HasKey( targetDisplayKey ) )
        {
            targetDisplay = PlayerPrefs.GetInt( targetDisplayKey );
        }
        
        if( PlayerPrefs.HasKey( postProcessKey ) )
        {
            postProcessEnabled = PlayerPrefs.GetInt( postProcessKey ) > 0;
        }

        postProcessVolume.enabled = postProcessEnabled;
    }
}
