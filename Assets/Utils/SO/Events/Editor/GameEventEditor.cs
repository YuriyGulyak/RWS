using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( GameEvent ) )]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var gameEvent = (GameEvent)target;
        if( GUILayout.Button( "Raise" ) )
        {
            gameEvent.Raise();
        }
    }
}