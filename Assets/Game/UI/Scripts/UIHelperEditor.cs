#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( UIHelper ) )]
public class UIHelperEditor : Editor
{
    public float scrollSensitivity = 2f;

    
    UIHelper uiHelper;
    

    void OnEnable()
    {
        uiHelper = (UIHelper)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();
        if( GUILayout.Button( "Apply" ) )
        {
            uiHelper.Apply();
        }
    }
}

#endif
