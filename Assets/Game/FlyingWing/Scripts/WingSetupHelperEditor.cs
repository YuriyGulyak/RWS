#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( WingSetupHelper ) )]
public class WingSetupHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();
        if( GUILayout.Button( "Update" ) )
        {
            wingSetupHelper.UpdateSections();
        }
    }

    //----------------------------------------------------------------------------------------------------

    void OnEnable()
    {
        wingSetupHelper = (WingSetupHelper)target;
        //wingSetupHelper.UpdateSections();
    }

    //----------------------------------------------------------------------------------------------------

    WingSetupHelper wingSetupHelper;
}

#endif