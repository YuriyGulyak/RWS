// https://answers.unity.com/questions/1611466/auto-grass-placement-not-working-properly.html
// Modified for bushes placement

using UnityEngine;
using UnityEditor;
 
public class GrassCreator : EditorWindow
{ 
    public Terrain terrain;
    public int detailIndex = 0;

    public float density = 0.2f;
    public int detailMin = 1;
    public int detailMax = 16;
    
    
    [MenuItem("Window/Terrain/Mass Grass Placement")]
    static void Init()
    {
        var window = (GrassCreator)GetWindow( typeof( GrassCreator ) );
        window.Show();
        window.titleContent = new GUIContent( "Mass Grass Placement" );
        window.Focus();
        window.ShowUtility();

        if( Selection.activeGameObject && Selection.activeGameObject.GetComponent<Terrain>() )
        {
            window.terrain = Selection.activeGameObject.GetComponent<Terrain>();
        }
    }
 
    
    void OnGUI()
    {
        EditorGUILayout.Separator();
        
        var thisSerializedObject = new SerializedObject( this );
        var terrainSerializedProperty = thisSerializedObject.FindProperty( "terrain" );
        EditorGUILayout.PropertyField( terrainSerializedProperty, new GUIContent( "Terrain Object:", "Place your terrain object in here." ), true );
        thisSerializedObject.ApplyModifiedProperties();

        if( terrain != null )
        {
            detailIndex = EditorGUILayout.IntField( "Detail Index to Place:", detailIndex );
            detailIndex = Mathf.Clamp( detailIndex, 0, terrain.terrainData.detailPrototypes.Length - 1 );

            detailMin = EditorGUILayout.IntField( "Detail Min:", detailMin );
            detailMin = Mathf.Clamp( detailMin, 1, 16 );
            
            detailMax = EditorGUILayout.IntField( "Detail Max:", detailMax );
            detailMax = Mathf.Clamp( detailMax, 1, 16 );
            
            density = EditorGUILayout.Slider( "Density", density, 0f, 1f );
            
            thisSerializedObject.ApplyModifiedProperties();

            EditorGUILayout.Separator();
            
            if( GUILayout.Button( "Mass Place Grass" ) )
            {
                CreateGrass();
            }
        }
    }
 
    
    void CreateGrass()
    {
        var detailWidth = terrain.terrainData.detailResolution;
        var detailHeight = detailWidth;
        var newDetailLayer = new int[ detailWidth, detailHeight ];
        
        for( var x = 0; x < detailWidth; x++ )
        {
            for( var y = 0; y < detailHeight; y++ )
            {
                if( Random.value > 1f - density )
                {
                    newDetailLayer[ x, y ] = Random.Range( detailMin, detailMax );
                }
            }
        }

        terrain.terrainData.SetDetailLayer( 0, 0, detailIndex, newDetailLayer );
    }
}