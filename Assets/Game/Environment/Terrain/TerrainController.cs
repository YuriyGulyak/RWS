using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField]
    Terrain terrain = null;
    
    //----------------------------------------------------------------------------------------------------
    
    public int GrassQualityLevel
    {
        get => grassQualityLevel;
        set
        {
            grassQualityLevel = Mathf.Clamp( value, 0, grassQualitySettings.Length - 1 );
            SetGrassQuality( grassQualitySettings[ grassQualityLevel ] );
        }
    }

    //----------------------------------------------------------------------------------------------------

    void OnValidate()
    {
        if( !terrain )
        {
            terrain = GetComponentInChildren<Terrain>();
        }
    }
    
    
    struct GrassQualitySettings
    {
        public float detailObjectDistance;
        public float detailObjectDensity;
    }

    GrassQualitySettings[] grassQualitySettings = 
    {
        // Off
        new GrassQualitySettings
        {
            detailObjectDistance = 0f,
            detailObjectDensity = 0f
        },
        // Low
        new GrassQualitySettings
        {
            detailObjectDistance = 250f,
            detailObjectDensity = 0.25f
        },
        // Normal
        new GrassQualitySettings
        {
            detailObjectDistance = 500f,
            detailObjectDensity = 0.5f
        },
        // High
        new GrassQualitySettings
        {
            detailObjectDistance = 1000f,
            detailObjectDensity = 1f
        }
    };
    
    int grassQualityLevel;

    
    void SetGrassQuality( GrassQualitySettings qualitySettings )
    {
        terrain.detailObjectDistance = qualitySettings.detailObjectDistance;
        terrain.detailObjectDensity = qualitySettings.detailObjectDensity;
    }
}
