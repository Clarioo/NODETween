namespace Utils.UIToolkit
{
    #if UNITY_EDITOR
    
    using System.Linq;
    using UnityEditor;
    using UnityEngine.UIElements;

    public static class EditorWindowHelper
    {
        /// <summary>
        /// Use this in combination with ConfigContainer to
        /// replace a placeholder at index in visualElement with container
        /// </summary>
        /// <param name="element">Container to be inserted</param>
        /// <param name="index">Index of the placeholder</param>
        /// <param name="visualElement">Visual element which contains the placeholder</param>
        public static void ReplaceWithVisualElementAtInVisualElement(VisualElement element, int index, VisualElement visualElement)
        {
            visualElement.RemoveAt(index);
            visualElement.Insert(index, element);
        }

        /// <summary>
        /// Returns a container with a property to be placed in UI
        /// </summary>
        /// <param name="serializedObject">The object which contains the property</param>
        /// <param name="propertyNameOf">Name of the property to be put in the container</param>
        /// <param name="id">Name of the container</param>
        /// <returns>Container with the property</returns>
        public static IMGUIContainer ConfigContainer(SerializedObject serializedObject, string propertyNameOf, string id)
        {
            var container = new IMGUIContainer(() =>
            {
                var serializedProperty = serializedObject.FindProperty(propertyNameOf);
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedProperty);
                serializedObject.ApplyModifiedProperties();
            });
            container.name = id;
            return container;
        }
        
        /// <summary>
        /// Updating given container with a property
        /// </summary>
        /// <param name="serializedObject">Object with property to serialize</param>
        /// <param name="propertyNameOf">Property name</param>
        /// <param name="container">Container to update events</param>
        /// <returns>Updated container</returns>
        public static IMGUIContainer UpdateIMGUIContainer(SerializedObject serializedObject, string propertyNameOf, IMGUIContainer container)
        {
            container.onGUIHandler = () =>
            {
                var serializedProperty = serializedObject.FindProperty(propertyNameOf);
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedProperty);
                serializedObject.ApplyModifiedProperties();
            };
            return container;
        }
        
        /// <summary>
        /// Setup pathButton as a path picker which connects the path to pathField
        /// </summary>
        /// <param name="pathButton">Button which will trigger the path picking</param>
        /// <param name="pathField">Text field which will contain the path</param>
        /// <returns>Setup path picking button</returns>
        public static Button SetupPathPicker(Button pathButton, TextField pathField)
        {
            pathButton.clickable.clicked += () =>
            {
                var path = EditorUtility.OpenFolderPanel("Choose a path", "Assets", "");
                var elements = path.Split('/').ToList();
                path = "Assets";
                var save = false;
                foreach (var element in elements)
                {
                    if (save)
                    {
                        path += '/' + element;
                        continue;
                    }
                
                    save = element == "Assets";
                }
                pathField.value = path;
            };

            return pathButton;
        }
    }
  #endif
}
