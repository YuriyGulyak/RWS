// https://gamedev.stackexchange.com/questions/141302/how-do-i-draw-lines-to-a-custom-inspector/142326

#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( RateProfile ) )]
public class RateProfileEditor : Editor
{
    readonly Color backgroundColor = new Color32( 30, 30, 30, 255 );
    readonly Color guideColor = new Color32( 50, 50, 50, 255 );
    readonly Color rollColor = new Color32( 200, 0, 0, 255 );
    readonly Color pitchColor = new Color32( 0, 200, 0, 255 );

    RateProfile rateProfile;
    Material material;
    

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EditorGUILayout.Separator();

        GUILayout.BeginHorizontal();
        
        var layoutRect = GUILayoutUtility.GetAspectRect( 1f );
        
        if( Event.current.type == EventType.Repaint )
        {
            GUI.BeginClip( layoutRect );
            GL.PushMatrix();
            
            GL.Clear( true, false, Color.black );
            material.SetPass( 0 );

            
            // Background
            GL.Begin( GL.QUADS );
            GL.Color( backgroundColor );
            GL.Vertex3( 0f, 0f, 0f );
            GL.Vertex3( layoutRect.width, 0f, 0f );
            GL.Vertex3( layoutRect.width, layoutRect.height, 0f );
            GL.Vertex3( 0, layoutRect.height, 0f );
            GL.End();

            
            GL.Begin( GL.LINES );
            
            // Guides
            GL.Color( guideColor );
            GL.Vertex3( layoutRect.width / 2f, 0f, 0f );
            GL.Vertex3( layoutRect.width / 2f, layoutRect.height, 0f );
            GL.Vertex3( 0f, layoutRect.height / 2f, 0f );
            GL.Vertex3( layoutRect.width, layoutRect.height / 2f, 0f );

            // Rate curves
            void DrawCurve( Func<float,float> evaluate, float width, float height, int resolution )
            {
                for( var i = 0; i < resolution; i++ )
                {
                    var xNorm1 = (float)i / resolution;
                    var xNorm2 = (float)( i + 1 ) / resolution;

                    var xPos1 = xNorm1 * width;
                    var xPos2 = xNorm2 * width;

                    var yPos1 = evaluate( Mathf.Lerp( 1f, -1f, xNorm1 ) ) * height / 2f + height / 2f;
                    var yPos2 = evaluate( Mathf.Lerp( 1f, -1f, xNorm2 ) ) * height / 2f + height / 2f;

                    GL.Vertex3( xPos1, yPos1, 0f );
                    GL.Vertex3( xPos2, yPos2, 0f );
                }
            }
            GL.Color( rollColor );
            DrawCurve( rateProfile.EvaluateRoll, layoutRect.width, layoutRect.height, 100 );
            GL.Color( pitchColor );
            DrawCurve( rateProfile.EvaluatePitch, layoutRect.width, layoutRect.height, 100 );
            
            GL.End();
            
            
            GL.PopMatrix();
            GUI.EndClip();
        }

        GUILayout.EndHorizontal();
        
        GUILayout.Label( "Red – Roll; Green – Pitch" );
        
        
        EditorGUILayout.Separator();
    }
   
    
    void OnEnable()
    {
        rateProfile = (RateProfile)target;
        material = new Material( Shader.Find( "Hidden/Internal-Colored" ) );
    }
}

#endif