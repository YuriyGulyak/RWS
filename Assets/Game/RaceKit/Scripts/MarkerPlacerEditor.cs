#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarkerPlacer))]
public class MarkerPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();
        if( GUILayout.Button( "Place" ) )
        {
            markerPlacer.PlaceMarkers();
        }
    }
    
    
    MarkerPlacer markerPlacer;

    void OnEnable()
    {
        markerPlacer = (MarkerPlacer)target;
        EditorUtility.SetDirty( markerPlacer );
    }
}
#endif