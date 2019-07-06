using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeightmapsLoader))]
[CanEditMultipleObjects]
public class HeightmapsLoaderEditor : Editor
{
    SerializedProperty heightMapTexture;
    SerializedProperty smoothPasses;

    void OnEnable()
    {
        heightMapTexture = serializedObject.FindProperty("heightMapTexture");
        smoothPasses = serializedObject.FindProperty("smoothPasses");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        HeightmapsLoader terrain = (HeightmapsLoader)target;

        #region Inspector GUI
        EditorGUILayout.PropertyField(heightMapTexture);
        EditorGUILayout.IntSlider(smoothPasses, 0, 10, new GUIContent("Smooth Passes"));
        if (GUILayout.Button("Load Texture"))
        {
            terrain.LoadTexture();
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
    }
}
