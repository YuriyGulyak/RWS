// !! Need set execution order before default time fot this script !!

using System;
using Boo.Lang;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace RWS
{
    public class GraphicsManager : Singleton<GraphicsManager>
    {
        [SerializeField]
        DisplayManger displayManger = null;

        [SerializeField]
        PostProcessVolume postProcessVolume = null;

        [SerializeField]
        TerrainController terrainController = null;
        
        //----------------------------------------------------------------------------------------------------

        public string[] QualityNames => QualitySettings.names;

        public int QualityLevel
        {
            get => QualitySettings.GetQualityLevel();
            set => QualitySettings.SetQualityLevel( value );
        }

        
        public int GrassQualityLevel
        {
            get => terrainController.GrassQualityLevel;
            set => terrainController.GrassQualityLevel = value;
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
            var resolutionWithoutRefreshRate = new Resolution { width = Screen.currentResolution.width, height = Screen.currentResolution.height };
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
                var resolutionWithoutRefreshRate = new Resolution { width = resolution.width, height = resolution.height };
                if( !resolutions.Contains( resolutionWithoutRefreshRate ) )
                {
                    resolutions.Add( resolutionWithoutRefreshRate );
                }
            }

            return resolutions;
        }

        public void LoadAndApllyPlayerPrefs()
        {
            QualityLevel = PlayerPrefs.GetInt( qualityLevelKey, 2 );
            GrassQualityLevel = PlayerPrefs.GetInt( grassQualityLevelKey, 3 );
            PostProcess = PlayerPrefs.GetInt( postProcessKey, 1 ) > 0;
            TargetDisplay = PlayerPrefs.GetInt( targetDisplayKey, 0 );
            VSync = PlayerPrefs.GetInt( vSyncKey, 1 ) > 0;
            FpsLimit = PlayerPrefs.GetInt( fpsLimitKey, 60 );
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetInt( qualityLevelKey, QualityLevel );
            PlayerPrefs.SetInt( grassQualityLevelKey, GrassQualityLevel );
            PlayerPrefs.SetInt( postProcessKey, PostProcess ? 1 : 0 );
            PlayerPrefs.SetInt( targetDisplayKey, TargetDisplay );
            PlayerPrefs.SetInt( vSyncKey, VSync ? 1 : 0 );
            PlayerPrefs.SetInt( fpsLimitKey, FpsLimit );
        }

        //----------------------------------------------------------------------------------------------------

        readonly string qualityLevelKey = "QualityLevel";
        readonly string grassQualityLevelKey = "GrassQualityLevel";
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
}