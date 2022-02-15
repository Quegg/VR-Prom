using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GuidingController))]
public class GuidingControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        if (GUILayout.Button("Generate Class Stubs"))
        {
            ((GuidingController)target).ParseAndGenerateStubs();
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Assign Classes"))
        {
            ((GuidingController)target).AssignStubs();
        }
        
        GUILayout.Space(5);
        if (GUILayout.Button("Create Error Event"))
        {
            ((GuidingController)target).CreateErrorEvent();
        }
        GUILayout.EndVertical();
    }
}
