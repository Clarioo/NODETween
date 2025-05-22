namespace AnimationSystem.CustomDrawers
{
    #if !ODIN_INSPECTOR
    using Runtime.Graph.Animations.CreationTools;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Utils.GUILogger;

    [CustomPropertyDrawer(typeof(AnimationGraphCreator))]
    public class AnimationGraphCreatorDrawer : PropertyDrawer
    {
        private float VerticalSpacing => EditorGUIUtility.standardVerticalSpacing * 2;
        
        protected float currentDrawHeight = 0f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Begin property to handle multi-object editing correctly
            EditorGUI.BeginProperty(position, label, property);
            var targetObject = property.serializedObject.targetObject;
            var obj = GetTargetObjectWithProperty(targetObject, property.propertyPath);
            
            // Create a foldout to show or hide the content of the serialized class
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), 
                property.isExpanded, label);            
            
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                currentDrawHeight = EditorGUIUtility.singleLineHeight;
                
                // Get the properties relative to this serialized class
                SerializedProperty animableObjectsProperty = property.FindPropertyRelative("AnimableObjects");
                SerializedProperty parametersContainerProperty = property.FindPropertyRelative("ParametersContainer");
                SerializedProperty graphProperty = property.FindPropertyRelative("SampleGraph");

                // Draw the default property drawer for each field
                var animableHeight = EditorGUI.GetPropertyHeight(animableObjectsProperty, true);
                Rect animableRect = new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, animableHeight);
                EditorGUI.PropertyField(animableRect, animableObjectsProperty, true);
                MoveNext(animableHeight);

                var parametersHeight = EditorGUI.GetPropertyHeight(parametersContainerProperty, true);
                Rect parametersRect = new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, parametersHeight);
                EditorGUI.PropertyField(parametersRect, parametersContainerProperty, true);
                MoveNext(parametersHeight);
                
                Rect graphRect = new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(graphRect, graphProperty);
                MoveNext(EditorGUIUtility.singleLineHeight);
                
                if(GUI.Button(new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight), "Show Graph"))
                {
                    MethodInfo method = obj.GetType().GetMethod("ShowGraphFromEditor");
                    method.Invoke(obj, null);
                }
                MoveNext(EditorGUIUtility.singleLineHeight);
                
                if(GUI.Button(new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight), "Create Graph"))
                {
                    MethodInfo method = obj.GetType().GetMethod("CreateGraphFromEditor");
                    method.Invoke(obj, null);
                }
                MoveNext(EditorGUIUtility.singleLineHeight);
                
                if(GUI.Button(new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight), "Check Configuration"))
                {
                    MethodInfo method = obj.GetType().GetMethod("CheckIfObjectsMatchingGraphParametersFromEditor");
                    method.Invoke(obj, null);
                }
                MoveNext(EditorGUIUtility.singleLineHeight);                
                
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                graphProperty.serializedObject.ApplyModifiedProperties();
                
                GUILogger.DrawLogs(obj, new Rect(position.x, position.y + currentDrawHeight + VerticalSpacing, position.width, EditorGUIUtility.singleLineHeight), VerticalSpacing);
                EditorGUI.indentLevel--;
            }

            // End property
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("AnimableObjects"), true) + VerticalSpacing;
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ParametersContainer"), true) + VerticalSpacing;
                height += EditorGUIUtility.singleLineHeight + VerticalSpacing; // for the graph property
                height += EditorGUIUtility.singleLineHeight + VerticalSpacing; // for the show graph button
                height += EditorGUIUtility.singleLineHeight + VerticalSpacing; // for the create graph button
                height += EditorGUIUtility.singleLineHeight + VerticalSpacing; // for the check configuration button
            }

            return height;
        }
        
        private void MoveNext(float height)
        {
            currentDrawHeight += height + VerticalSpacing;
        }
        
        // Helper method to get the target object from the serialized property path
        private object GetTargetObjectWithProperty(object obj, string path)
        {
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element == "Array")
                    continue;
                if (element.StartsWith("data["))
                    continue;

                var field = obj.GetType().GetField(element, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null)
                    return null;

                obj = field.GetValue(obj);
            }
            return obj;
        }
        

    }
    
    #endif
}
