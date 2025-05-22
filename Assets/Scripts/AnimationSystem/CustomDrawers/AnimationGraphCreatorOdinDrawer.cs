namespace AnimationSystem.CustomDrawers
{
    #if ODIN_INSPECTOR
    using Runtime.Graph.Animations.CreationTools;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;
    using Utils.GUILogger;

    public class AnimationGraphCreatorOdinDrawer : OdinValueDrawer<AnimationGraphCreator>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Draw base
            CallNextDrawer(label);
            
            // Get object to log
            var obj = this.Property.ValueEntry.WeakSmartValue;
            
            // Start drawing loggers
            EditorGUILayout.BeginVertical();
            var buttonRect = new Rect(EditorGUILayout.GetControlRect());
            buttonRect.height = EditorGUIUtility.singleLineHeight;
            
            // Draw logs from object
            GUILogger.DrawLogs(obj, buttonRect, EditorGUIUtility.standardVerticalSpacing);
            EditorGUILayout.EndVertical();
        }
    }
    #endif
}
