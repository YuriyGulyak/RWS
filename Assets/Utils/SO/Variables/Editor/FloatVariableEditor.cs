// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor( typeof( FloatVariable ) )]
// public class FloatVariableEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();
//
//         var floatVariable = (FloatVariable)target;
//         Debug.Log( floatVariable.RuntimeValue );
//         EditorGUILayout.FloatField( "RuntimeValue", floatVariable.RuntimeValue );
//     }
// }