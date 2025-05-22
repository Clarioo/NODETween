namespace AnimationSystem.CustomDrawers
{
    #if !ODIN_INSPECTOR
    using Runtime.Logic.Animation;
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(AnimationRunner), true)]
    public class AnimationRunnerEditor : Editor
    {
        private float _animationProgress;

        private GUIStyle titleStyle;

        public override async void OnInspectorGUI()
        {
            AnimationRunner baseScript = (AnimationRunner)target;
            serializedObject.Update();
            
            // Create a label for the Preview section
            titleStyle = new GUIStyle()
            {
                fontStyle = FontStyle.Bold,
                fontSize = 22,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("Creator", titleStyle);
            
            SerializedProperty creatorProperty = serializedObject.FindProperty("animationGraphCreator");
            
            // draw the default inspector
            EditorGUILayout.PropertyField(creatorProperty, true);
            
            EditorGUI.BeginChangeCheck();
            var property = serializedObject.FindProperty("animationProgress");
            
            GUILayout.Label("Preview", titleStyle);
            
            property.floatValue = EditorGUILayout.Slider("Animation Progress", property.floatValue, 0f, 1f);
            
            // Check if slider has changed
            if (EditorGUI.EndChangeCheck())
            {
                // Call the base class method if the value changes
                baseScript.EvaluatePreview(property.floatValue);
                _animationProgress = property.floatValue;
            }
            
            // Check if the value has changed externally to repaint the editor
            if (Math.Abs(_animationProgress - property.floatValue) > 0.001f)
            {
                _animationProgress = property.floatValue;
                Repaint();
            }
            
            // Create buttons to Preview
            if (GUILayout.Button("Enable Preview"))
            {
                await baseScript.EnablePreview();
            }
            if (baseScript.IsPreviewing)
            {
                if (baseScript.IsPreviewPlaying)
                {
                    if (GUILayout.Button("Pause Preview"))
                    {
                        await baseScript.PlayPausePreview();
                    }
                }
                else
                {
                    if (GUILayout.Button("Play Preview"))
                    {
                        await baseScript.PlayPausePreview();
                    }
                }
            }
            if (GUILayout.Button("Disable Preview"))
            {
                baseScript.DisablePreview();
            }
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target); // Ensure changes are applied
            }
        }
    }
    #endif
}