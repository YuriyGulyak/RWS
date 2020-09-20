using System;
using Unity.Mathematics;
using UnityEngine;

public class PixelateImageEffect : MonoBehaviour
{
    [SerializeField]
    Material material = null;

    [SerializeField]
    float minHeight = 128f;
    
    [SerializeField, Range( 0f, 1f )]
    float intensity = 0f;


    public float Intensity
    {
        get => intensity;
        set => intensity = math.saturate( value );
    }


    int widthId;
    int heightId;
    float widthBackup;
    float heightBackup;
    
    
    void OnEnable()
    {
        widthId = Shader.PropertyToID( "_PixelCountU" );
        heightId = Shader.PropertyToID( "_PixelCountV" );
        
        widthBackup = material.GetFloat( widthId );
        heightBackup = material.GetFloat( heightId );
    }

    void OnDisable()
    {
        material.SetFloat( widthId, widthBackup );
        material.SetFloat( heightId, heightBackup );
    }

    void OnRenderImage( RenderTexture src, RenderTexture dest )
    {
        var scale = 1f - intensity;
        
        if( src.height * scale >= minHeight )
        {
            material.SetFloat( widthId, src.width * scale );
            material.SetFloat( heightId, src.height * scale );
        }
        
        Graphics.Blit( src, dest, material );
    }
}