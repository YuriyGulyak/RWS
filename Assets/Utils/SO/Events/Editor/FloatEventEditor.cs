using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( FloatEvent ) )]
public class FloatEventEditor : Editor
{
    float debugValue = default;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        debugValue = EditorGUILayout.FloatField( "Debug value:", debugValue );

        var floatEvent = (FloatEvent)target;
        if( GUILayout.Button( "Raise" ) )
        {
            floatEvent.Raise( debugValue );
        }
    }
}