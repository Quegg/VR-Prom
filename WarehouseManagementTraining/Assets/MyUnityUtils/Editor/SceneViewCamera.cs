using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * When this script is places inside a project, it allows you to save the scene views camera position
 * by pressing crtl+num1, crtl+num2, crtl+num3.
 * The positions can be loaded by pressing num1, num2, or num3.
 */
[InitializeOnLoad]
public static class SceneViewCamera
{
    
    static SceneViewCamera()
    {
        SceneView.duringSceneGui += CheckInput;
    }
    
    static void CheckInput(SceneView scene)
    {
        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        Event e = Event.current;
        if (e.GetTypeForControl(controlId) == EventType.KeyDown &&e.control&&e.keyCode == KeyCode.Keypad1)
        {
            
            Vector3 position = SceneView.lastActiveSceneView.pivot;
            EditorPrefs.SetFloat("onePx",position.x);
            EditorPrefs.SetFloat("onePy",position.y);
            EditorPrefs.SetFloat("onePz",position.z);
            Vector3 rotationEulers =SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
            EditorPrefs.SetFloat("oneRx",rotationEulers.x);
            EditorPrefs.SetFloat("oneRy",rotationEulers.y);
            EditorPrefs.SetFloat("oneRz",rotationEulers.z);
            
            
        }
        if (e.GetTypeForControl(controlId) == EventType.KeyDown &&e.control&&e.keyCode == KeyCode.Keypad2)
        {
            Vector3 position = SceneView.lastActiveSceneView.pivot;
            EditorPrefs.SetFloat("twoPx",position.x);
            EditorPrefs.SetFloat("twoPy",position.y);
            EditorPrefs.SetFloat("twoPz",position.z);
            Vector3 rotationEulers =SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
            EditorPrefs.SetFloat("twoRx",rotationEulers.x);
            EditorPrefs.SetFloat("twoRy",rotationEulers.y);
            EditorPrefs.SetFloat("twoRz",rotationEulers.z);
        }
        if (e.GetTypeForControl(controlId) == EventType.KeyDown &&e.control&&e.keyCode == KeyCode.Keypad3)
        {
            Vector3 position = SceneView.lastActiveSceneView.pivot;
            EditorPrefs.SetFloat("threePx",position.x);
            EditorPrefs.SetFloat("threePy",position.y);
            EditorPrefs.SetFloat("threePz",position.z);
            Vector3 rotationEulers =SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
            EditorPrefs.SetFloat("threeRx",rotationEulers.x);
            EditorPrefs.SetFloat("threeRy",rotationEulers.y);
            EditorPrefs.SetFloat("threeRz",rotationEulers.z);
        }
    
        if (e.GetTypeForControl(controlId) == EventType.KeyDown &&!e.control&&e.keyCode == KeyCode.Keypad1)
        {
            
            if (EditorPrefs.HasKey("onePx"))
            {
                
                Vector3 position = new Vector3(EditorPrefs.GetFloat("onePx"),EditorPrefs.GetFloat("onePy"),EditorPrefs.GetFloat("onePz"));
                
                Vector3 rotationEulers = new Vector3(EditorPrefs.GetFloat("oneRx"),EditorPrefs.GetFloat("oneRy"),EditorPrefs.GetFloat("oneRz"));
                
                SceneView.lastActiveSceneView.LookAt(position,Quaternion.Euler(rotationEulers));
            }
        }
        
        if (e.GetTypeForControl(controlId) == EventType.KeyDown &&!e.control&&e.keyCode == KeyCode.Keypad2)
        {
            Vector3 position = new Vector3(EditorPrefs.GetFloat("twoPx"),EditorPrefs.GetFloat("twoPy"),EditorPrefs.GetFloat("twoPz"));
                
            Vector3 rotationEulers = new Vector3(EditorPrefs.GetFloat("twoRx"),EditorPrefs.GetFloat("twoRy"),EditorPrefs.GetFloat("twoRz"));
                
            SceneView.lastActiveSceneView.LookAt(position,Quaternion.Euler(rotationEulers));
        }
        
        if (e.GetTypeForControl(controlId) == EventType.KeyDown &&!e.control&&e.keyCode == KeyCode.Keypad3)
        {
            Vector3 position = new Vector3(EditorPrefs.GetFloat("threePx"),EditorPrefs.GetFloat("threePy"),EditorPrefs.GetFloat("threePz"));
                
            Vector3 rotationEulers = new Vector3(EditorPrefs.GetFloat("threeRx"),EditorPrefs.GetFloat("threeRy"),EditorPrefs.GetFloat("threeRz"));
                
            SceneView.lastActiveSceneView.LookAt(position,Quaternion.Euler(rotationEulers));
        }
        
        
    }
}
